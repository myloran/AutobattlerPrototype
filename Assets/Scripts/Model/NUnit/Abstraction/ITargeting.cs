using System.Collections.Generic;
using Shared.Primitives;

namespace Model.NUnit.Abstraction {
  public interface ITargeting {
    IEnumerable<IUnit> ArrivingTargets { get; set; }
    IEnumerable<Coord> ArrivingTargetCoords { get; } //to test determinism
    IUnit Target { get; }
    Coord TargetCoord { get; } //to test determinism
    bool TargetExists { get; }
    bool IsTaunted { get; }
    void ClearTarget();
    void ChangeTargetTo(IUnit unit, bool isTaunted = false);
  }
}