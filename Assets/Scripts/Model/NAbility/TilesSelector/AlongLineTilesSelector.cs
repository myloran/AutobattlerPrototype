using System.Collections.Generic;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NAbility.TilesSelector {
  public class AlongLineTilesSelector {
    public AlongLineTilesSelector(IMovement unit) {
      this.unit = unit;
    }

    public IEnumerable<Coord> Select(Coord targetCoord, AiContext context) {
      foreach (Coord coord in Bresenham.New(targetCoord, unit.Coord)) {
        yield return coord;
        // var additionalTarget = context.TryGetUnit(coord);
        // if (additionalTarget != null && additionalTarget.Player == target.Player) yield return additionalTarget;
      }
    }

    readonly IMovement unit;
  }
}