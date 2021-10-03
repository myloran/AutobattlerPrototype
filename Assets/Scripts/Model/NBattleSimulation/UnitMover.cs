using System.Collections.Generic;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NBattleSimulation {
  public class UnitMover {
    public UnitMover(Player player, List<IUnit> boardUnits, List<IUnit> benchUnits) {
      this.player = player;
      this.boardUnits = boardUnits;
      this.benchUnits = benchUnits;
    }

    public void MoveUnit(Coord from, Coord to) {
      var fromList = from.IsBench() ? benchUnits : boardUnits;
      var toList = to.IsBench() ? benchUnits : boardUnits;
      var fromUnit = player.GetUnit(from).Unit;
      var unitPair = player.GetUnit(to);
      var toUnit = unitPair.Unit;

      if (unitPair.HasUnit) {
        fromList.Remove(fromUnit);
        toList.Remove(toUnit);
        fromList.Add(toUnit);
        toList.Add(fromUnit);
        fromUnit.StartingCoord = to;
        toUnit.StartingCoord = from;
      }
      else {
        fromList.Remove(fromUnit);
        toList.Add(fromUnit);
        fromUnit.StartingCoord = to;
      }
    }

    readonly Player player;
    readonly List<IUnit> boardUnits;
    readonly List<IUnit> benchUnits;
  }
}