using System.Collections.Generic;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAbility.Abstraction {
  public class CompositeEffect : IEffect {
    public CompositeEffect(IEffect[] effects) {
      this.effects = effects;
    }

    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var effect in effects) effect.Apply(context, units);
    }

    readonly IEffect[] effects;
  }
}