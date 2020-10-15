using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NUnit.Abstraction;
using Shared.Exts;

namespace Model.NAbility {
  public class SingleTargetsSelector : ITargetsSelector {
    public IEnumerable<IUnit> Select(IUnit target) => target.AsEnumerable();
  }
}