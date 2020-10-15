using System.Collections.Generic;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAbility.Abstraction {
  public interface ITargetsSelector {
    IEnumerable<IUnit> Select(IUnit target);
  }
}