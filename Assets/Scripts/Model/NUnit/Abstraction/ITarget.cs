using System.Collections.Generic;

namespace Model.NUnit.Abstraction {
  public interface ITarget {
    IUnit Target { get; }
    bool TargetExists { get; }
    void ClearTarget();
    void ChangeTargetTo(IUnit unit);
    (bool, IUnit) FindNearestTarget(IEnumerable<IUnit> units);
  }
}