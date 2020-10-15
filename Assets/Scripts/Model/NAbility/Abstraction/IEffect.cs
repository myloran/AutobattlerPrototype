using System.Collections.Generic;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAbility.Abstraction {
  public interface IEffect {
    void Apply(AiContext context, IEnumerable<IUnit> units);
  }
}