using System.Collections.Generic;

namespace Model.NUnit.Abstraction {
  public interface ITarget {
    IEnumerable<IUnit> ArrivingTargets { get; set; }
    IUnit Target { get; }
    bool TargetExists { get; }
    void ClearTarget();
    void ChangeTargetTo(IUnit unit);
    IUnit FindNearestTarget(IEnumerable<IUnit> units);
  }
}