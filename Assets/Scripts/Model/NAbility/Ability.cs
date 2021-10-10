using System;
using System.Collections.Generic;
using System.Linq;
using Model.NAbility.Abstraction;
using Model.NAI.Commands;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAbility {
  public class Ability {
    [JsonIgnore] public readonly IUnit Unit;
    [JsonIgnore] public List<IUnit> TargetsSelected = new List<IUnit>();

    public Ability(IUnit unit, IMainTargetSelector targetSelector, IAdditionalTargetsSelector targetsSelector, List<IEffect> effects, 
      ITiming timing, bool isTimingOverridden = false, IEnumerable<Ability> nestedAbilities = null) {
      Unit = unit;
      this.targetSelector = targetSelector;
      this.targetsSelector = targetsSelector;
      this.effects = effects;
      this.timing = timing;
      this.isTimingOverridden = isTimingOverridden;
      this.nestedAbilities = nestedAbilities ?? new List<Ability>();
    }

    public void Cast(AiContext context) => Cast(Zero, context);
    void Cast(F32 time, AiContext context) => context.InsertCommand(time, new ExecuteAbilityCommand(this, context));
    public IUnit SelectTarget(AiContext context) => targetSelector.Select(context);

    public void Execute(AiContext context) {
      var target = targetSelector.Select(context);
      TargetsSelected = targetsSelector.Select(target, context).ToList();
      HandleTiming(context, TargetsSelected, ApplyEffects);
    }

    void HandleTiming(AiContext context, List<IUnit> targets, Action<AiContext, List<IUnit>> applyEffects) {
      if (!timing.HasNext()) return;

      if (timing.IsTimeReset || timing.GetNext() == Zero) {
        timing.IsTimeReset = false;
        timing.Tick();
        applyEffects(context, targets);
      }

      if (!timing.HasNext()) return;

      timing.IsTimeReset = true;
      Cast(timing.GetNext(), context);
    }

    void ApplyEffects(AiContext context, List<IUnit> targets) {
      foreach (var effect in effects)
        effect.Apply(context, targets);

      foreach (var ability in nestedAbilities) {
        ability.targetSelector = targetSelector;
        ability.targetsSelector = targetsSelector;
        
        //if target selector is overriden, we need to reevaluate targets
        //nestedAbility.Execute(context); //wasted performance, share result if target selector is the same
        if (ability.isTimingOverridden) {
          ability.HandleTiming(context, targets, ability.ApplyEffects);
        }
        else {
          foreach (var effect in ability.effects)
            effect.Apply(context, targets);
        }
      }
    }

    public void Reset() {
      timing.Reset();
      foreach (var ability in nestedAbilities) ability.Reset();
    }
    
    readonly IEnumerable<Ability> nestedAbilities;
    readonly List<IEffect> effects;
    readonly ITiming timing;
    readonly bool isTimingOverridden;
    IMainTargetSelector targetSelector;
    IAdditionalTargetsSelector targetsSelector;
  }
}