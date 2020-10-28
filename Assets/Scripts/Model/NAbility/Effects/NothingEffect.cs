using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAbility.Effects {
  public class NothingEffect : IEffect {
    public void Apply(AiContext context, IEnumerable<IUnit> units) { }
  }
}