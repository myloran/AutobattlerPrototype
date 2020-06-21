using System.Collections.Generic;
using System.Linq;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Exts;
using Shared.Poco;

namespace Model.NUnit.Components {
  public class CTarget : ITarget {
    public IUnit Target { get; private set; }

    public CTarget(IMovement movement) => this.movement = movement;

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
    
    public (bool, IUnit) FindNearestTarget(IEnumerable<IUnit> units) =>
      units.Any() 
        ? (true, units.MinBy(u => CoordExt.SqrDistance(movement.Coord, u.Coord))) 
        : (false, default);

    //TODO: log target without it's target?
    public override string ToString() => TargetExists ? $"Target coord: {Target.Coord}" : "";

    readonly IMovement movement;
  }
}