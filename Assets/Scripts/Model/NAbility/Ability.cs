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
    public readonly bool IsTimingOverridden;
    public readonly IEnumerable<Ability> NestedAbilities;
    public readonly IEffect Effect;
    public readonly ITiming Timing;
    public ITargetSelector TargetSelector;
    public ITargetsSelector TargetsSelector;
    
    public Ability(ITargetSelector targetSelector, ITargetsSelector targetsSelector, IEffect effect, ITiming timing, 
        bool isTimingOverridden = false, IEnumerable<Ability> nestedAbilities = null) {
      TargetSelector = targetSelector;
      TargetsSelector = targetsSelector;
      Effect = effect;
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
      Effect.Apply(context, targets);

      foreach (var nestedAbility in NestedAbilities) {
        if (nestedAbility.IsTimingOverridden && nestedAbility.Timing.HasNext()) {
          nestedAbility.TargetSelector = TargetSelector;
          nestedAbility.TargetsSelector = TargetsSelector;
          nestedAbility.Cast(nestedAbility.Timing.GetNext(), context);
        }
        else {
          nestedAbility.Effect.Apply(context, targets);
        }
      }

      if (!Timing.HasNext()) return;

      var time = Timing.GetNext();
      Cast(time, context);
    }
  }
}