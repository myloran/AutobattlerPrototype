using System.Collections.Generic;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NAbility.TilesSelector {
  public class AlongLineTilesSelector {
    public AlongLineTilesSelector(IUnit unit) {
      this.unit = unit;
    }

    public IEnumerable<Coord> Select(Coord targetCoord) {
      //TODO: expand coord to max ability range based on targetCoord
      var diff = targetCoord - unit.Coord;
      var extendedTarget = targetCoord + diff;

      while (CoordExt.SqrDistance(unit.Coord, extendedTarget) > unit.AbilitySqrRadius) 
        extendedTarget += diff;

      foreach (Coord coord in Bresenham.New(unit.Coord, extendedTarget)) {
        if (CoordExt.SqrDistance(unit.Coord, coord) > unit.AbilitySqrRadius) break;
        
        yield return coord;
      }
    }

    readonly IUnit unit;
  }
}