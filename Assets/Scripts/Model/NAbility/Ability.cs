using System;
using System.Collections.Generic;
using System.Linq;
using Model.NAbility.Abstraction;
using Model.NAI.Commands;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAbility {
  public class Ability {
    public readonly IUnit Unit;
    public readonly bool IsTimingOverridden;
    public readonly IEnumerable<Ability> NestedAbilities;
    public readonly List<IEffect> Effects;
    public readonly ITiming Timing;
    public ITargetSelector TargetSelector;
    public ITargetsSelector TargetsSelector;
    
    public Ability(IUnit unit, ITargetSelector targetSelector, ITargetsSelector targetsSelector, List<IEffect> effects, 
      ITiming timing, bool isTimingOverridden = false, IEnumerable<Ability> nestedAbilities = null) {
      Unit = unit;
      TargetSelector = targetSelector;
      TargetsSelector = targetsSelector;
      Effects = effects;
      Timing = timing;
      IsTimingOverridden = isTimingOverridden;
      NestedAbilities = nestedAbilities ?? new List<Ability>();
    }

    public void Cast(AiContext context) => Cast(Zero, context);
    public void Cast(F32 time, AiContext context) => context.InsertCommand(time, new ExecuteAbilityCommand(this, context));
    public IUnit SelectTarget(AiContext context) => TargetSelector.Select(context);

    public void Execute(AiContext context) {
      var target = TargetSelector.Select(context);
      var targets = TargetsSelector.Select(target).ToList();
      HandleTiming(context, targets, ApplyEffects);
    }

    void HandleTiming(AiContext context, List<IUnit> targets, Action<AiContext, List<IUnit>> applyEffects) {
      if (!Timing.HasNext()) return;

      if (Timing.IsTimeReset || Timing.GetNext() == Zero) {
        Timing.IsTimeReset = false;
        Timing.Tick();
        applyEffects(context, targets);
      }

      if (!Timing.HasNext()) return;

      Timing.IsTimeReset = true;
      Cast(Timing.GetNext(), context);
    }

    void ApplyEffects(AiContext context, List<IUnit> targets) {
      foreach (var effect in Effects)
        effect.Apply(context, targets);

      foreach (var ability in NestedAbilities) {
        ability.TargetSelector = TargetSelector;
        ability.TargetsSelector = TargetsSelector;
        
        //if target selector is overriden, we need to reevaluate targets
        //nestedAbility.Execute(context); //wasted performance, share result if target selector is the same
        if (ability.IsTimingOverridden) {
          ability.HandleTiming(context, targets, ability.ApplyEffects);
        }
        else {
          foreach (var effect in ability.Effects)
            effect.Apply(context, targets);
        }
      }
    }

    public void Reset() {
      Timing.Reset();
      foreach (var ability in NestedAbilities) ability.Reset();
    }
  }
}