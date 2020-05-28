using System.Collections.Generic;
using System.Linq;
using Shared;

namespace Model.NUnit {
  public class CTarget {
    public Unit Unit;

    public CTarget(CMovement movement) {
      this.movement = movement;
    }
    
    public bool Exists => Unit != null;

    public void FindNearestTarget(IEnumerable<Unit> units) {
      if (!units.Any()) return;
      
      Unit = units.MinBy(u => CoordExt.SqrDistance(movement.Coord, u.Movement.Coord));
    } 

    readonly CMovement movement;
  }
}