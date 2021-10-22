using System.Collections.Generic;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Model.NUnit.Abstraction {
  public interface ITargeting {
    IEnumerable<IUnit> ArrivingTargets { get; set; }
    IEnumerable<Coord> ArrivingTargetCoords { get; } //to test determinism
    IUnit Target { get; }
    Coord TargetCoord { get; } //to test determinism
    bool TargetExists { get; }
    F32 TauntEndTime { get; }
    bool IsTaunted(F32 currentTime);
    void Taunt(IUnit unit, F32 tauntEndTime);
    void ClearTarget();
    void ChangeTargetTo(IUnit unit);
  }
}