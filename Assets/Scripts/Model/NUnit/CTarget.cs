using System.Collections.Generic;
using System.Linq;
using Model.NUnit.Abstraction;
using Shared;

namespace Model.NUnit {
  public class CTarget : ITarget {
    public Unit Target { get; private set; }

    public CTarget(IMovement movement) => this.movement = movement;

    public bool TargetExists => Target != null;
    public void Reset() => Target = null;
    
    public void ClearTarget() {
      if (!TargetExists) return;
      
      Target.UnsubFromDeath(this);
      Target = null;
    }

    public void ChangeTargetTo(Unit unit) {
      ClearTarget();
      Target = unit;
      Target.SubToDeath(this);
    }
    
    public (bool, Unit) FindNearestTarget(IEnumerable<Unit> units) =>
      units.Any() 
        ? (true, units.MinBy(u => CoordExt.SqrDistance(movement.Coord, u.Coord))) 
        : (false, default);

    public static implicit operator Unit(CTarget target) => target.Target;

    public override string ToString() => TargetExists ? $"Target coord: {Target.Coord}" : "";

    readonly IMovement movement;
  }
}