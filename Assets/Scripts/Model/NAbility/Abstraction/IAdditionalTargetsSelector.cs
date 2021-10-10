using System.Collections.Generic;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAbility.Abstraction {
  public interface IAdditionalTargetsSelector {
    IEnumerable<IUnit> Select(IUnit target, AiContext context);
  }
}