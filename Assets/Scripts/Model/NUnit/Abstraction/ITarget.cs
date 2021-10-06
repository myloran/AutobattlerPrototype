using System.Collections.Generic;
using Shared.Primitives;

namespace Model.NUnit.Abstraction {
  public interface ITarget {
    IEnumerable<IUnit> ArrivingTargets { get; set; }
    IEnumerable<Coord> ArrivingTargetCoords { get; } //to test determinism
    IUnit Target { get; }
    Coord TargetCoord { get; } //to test determinism
    bool TargetExists { get; }
    void ClearTarget();
    void ChangeTargetTo(IUnit unit);
    IUnit FindNearestTarget(IEnumerable<IUnit> units);
  }
}