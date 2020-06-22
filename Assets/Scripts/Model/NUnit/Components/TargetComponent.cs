using System.Collections.Generic;
using System.Linq;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Exts;
using static Shared.Primitives.CoordExt;

namespace Model.NUnit.Components {
  public class TargetComponent : ITarget {
    public IUnit Target { get; private set; }

    public TargetComponent(IMovement movement) => this.movement = movement;

    public bool TargetExists => Target != null;
    public void Reset() => Target = null;
    
    public void ClearTarget() {
      if (!TargetExists) return;
      
      Target.UnsubFromDeath(this);
      Target = null;
    }

    public void ChangeTargetTo(IUnit unit) {
      ClearTarget();
      Target = unit;
      Target.SubToDeath(this);
    }
    
    public IUnit FindNearestTarget(IEnumerable<IUnit> units) =>
      units.MinBy(u => SqrDistance(movement.Coord, u.Coord));

    //TODO: log target without it's target?
    public override string ToString() => TargetExists ? $"Target coord: {Target.Coord}" : "";

    readonly IMovement movement;
  }
}