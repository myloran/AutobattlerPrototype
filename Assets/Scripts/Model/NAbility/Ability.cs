using System;
using System.Collections.Generic;
using System.Linq;
using Model.NAbility.Abstraction;
using Model.NAbility.TilesSelector;
using Model.NAI.Commands;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAbility {
  public class Ability {
    [JsonIgnore] public readonly IUnit Unit;
    [JsonIgnore] public IEnumerable<IUnit> TargetsSelected = new List<IUnit>(); //TODO: expose IEnumerable<Coord> for determinism testing
    [JsonIgnore] public IEnumerable<Coord> TilesSelected = new List<Coord>();

    public Ability(IUnit unit, EPlayer player, IMainTargetSelector targetSelector,
      IAdditionalTargetsSelector additionalTargetsSelector,
      AlongLineTilesSelector tilesSelector, IEffect effect,
      ITiming timing, bool isTimingOverridden = false, bool needRecalculateTarget = false,
      IEnumerable<Ability> nestedAbilities = null) {
      Unit = unit;
      this.player = player;
      this.targetSelector = targetSelector;
      this.additionalTargetsSelector = additionalTargetsSelector;
      this.tilesSelector = tilesSelector;
      this.effect = effect;
      this.timing = timing;
      this.isTimingOverridden = isTimingOverridden;
      this.needRecalculateTarget = needRecalculateTarget;
      this.nestedAbilities = nestedAbilities ?? new List<Ability>();
    }

    public void Cast(AiContext context) => Cast(Zero, context);
    void Cast(F32 time, AiContext context) => context.InsertCommand(time, new ExecuteAbilityCommand(this, context));
    public IUnit SelectTarget(AiContext context) => targetSelector.Select(context);

    public void Execute(AiContext context) {
      if (tilesSelector != null)
        SelectTargetsBasedOnTilesCached(context);
      else
        SelectTargets(context);

      HandleTiming(context, TargetsSelected, ApplyEffects);
    }

    void SelectTargets(AiContext context) {
      if (needRecalculateTarget)
        SelectTargetsBasedOnTargeting(context);
      else if (!isCached) {
        isCached = true;
        SelectTargetsBasedOnTargeting(context);
      }
    }

    void SelectTargetsBasedOnTargeting(AiContext context) {
      var target = targetSelector.Select(context);
      TargetsSelected = additionalTargetsSelector.Select(target, context);//TODO: if target is null, we do anything, consider do use last known target location instead
    }

    void SelectTargetsBasedOnTilesCached(AiContext context) {
      if (!isCached) {
        isCached = true;
        var target = targetSelector.Select(context);
        TilesSelected = tilesSelector.Select(target.Coord);
      }

      TargetsSelected = context.GetUnits(TilesSelected, player);
    }

    void HandleTiming(AiContext context, IEnumerable<IUnit> targets, Action<AiContext, IEnumerable<IUnit>> applyEffects) {
      if (!timing.InitialDelayHandled)
      {
        timing.InitialDelayHandled = true;

        if (timing.InitialDelay == Zero)
          ApplyEffectsNow(context, targets, applyEffects);
        else
          Cast(timing.InitialDelay, context);
      }
      else ApplyEffectsNow(context, targets, applyEffects);
    }

    void ApplyEffectsNow(AiContext context, IEnumerable<IUnit> targets, Action<AiContext, IEnumerable<IUnit>> applyEffects) {
      timing.TakeNext();
      applyEffects(context, targets);

      if (timing.HasNext()) 
        Cast(timing.Period, context);
    }

    void ApplyEffects(AiContext context, IEnumerable<IUnit> targets) {
      effect.Apply(context, targets);

      foreach (var ability in nestedAbilities) {
        ability.targetSelector = targetSelector;
        ability.additionalTargetsSelector = additionalTargetsSelector;
        ability.tilesSelector = tilesSelector;
        
        //if target selector is overriden, we need to reevaluate targets
        //nestedAbility.Execute(context); //wasted performance, share result if target selector is the same
        if (ability.isTimingOverridden)
          ability.HandleTiming(context, targets, ability.ApplyEffects);
        else
          effect.Apply(context, targets);
      }
    }

    public void Reset() {
      //TODO: create new ability if previous is not finished yet
      timing.Reset();
      foreach (var ability in nestedAbilities) ability.Reset();
      isCached = false;
      TargetsSelected = Enumerable.Empty<IUnit>();
      TilesSelected = Enumerable.Empty<Coord>();
    }
    
    readonly IEnumerable<Ability> nestedAbilities;
    readonly IEffect effect;
    readonly ITiming timing;
    readonly bool isTimingOverridden;
    readonly bool needRecalculateTarget;
    readonly EPlayer player;
    IMainTargetSelector targetSelector;
    IAdditionalTargetsSelector additionalTargetsSelector;
    AlongLineTilesSelector tilesSelector;
    bool isCached;
  }
}