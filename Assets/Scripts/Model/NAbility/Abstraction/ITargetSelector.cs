using System.Collections.Generic;
using Model.NUnit;

namespace Model.NAbility {
  public interface ITargetSelector {
    IEnumerable<Unit> Targets { get; }
    Unit Select();
  }
}