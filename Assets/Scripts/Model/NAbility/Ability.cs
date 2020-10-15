using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAbility {
  public class Ability {
    public Ability(ITargetSelector targetSelector, ITargetsSelector targetsSelector, IEffect effect) {
      this.targetSelector = targetSelector;
      this.targetsSelector = targetsSelector;
      this.effect = effect;
    }

    public void Cast(AiContext context) {
      var target = targetSelector.Select(context);
      var targets = targetsSelector.Select(target);
      effect.Apply(context, targets);
    }
    
    public IUnit SelectTarget(AiContext context) => targetSelector.Select(context);
    
    List<Ability> nestedAbilities = new List<Ability>();
    readonly ITargetSelector targetSelector;
    readonly ITargetsSelector targetsSelector;
    readonly IEffect effect;
  }
}