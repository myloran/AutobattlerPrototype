using System.Collections.Generic;
using Shared.Primitives;

namespace Shared.Abstraction {
  public class UnitMoveStrategy<T> {
    public UnitMoveStrategy(Dictionary<Coord, T> benchUnits, Dictionary<Coord, T> boardUnits, 
        IHandler<UnitCoordChanged<T>> unitCoordChangedHandler) {
      this.benchUnits = benchUnits;
      this.boardUnits = boardUnits;
      this.unitCoordChangedHandler = unitCoordChangedHandler;
    }
    
    public void MoveUnit(Coord from, Coord to) {
      var fromDict = from.IsBench() ? benchUnits : boardUnits;
      var toDict = to.IsBench() ? benchUnits : boardUnits;
      var unit = fromDict[from];
      var hasUnitAtDestination = toDict.ContainsKey(to);

      if (hasUnitAtDestination)
        SwapUnits(from, to, fromDict, toDict, unit);
      else
        MoveUnit(from, to, fromDict, toDict, unit);
    }

    void MoveUnit(Coord from, Coord to, Dictionary<Coord, T> fromDict,
        Dictionary<Coord, T> toDict, T fromUnit) {
      fromDict.Remove(from);
      unitCoordChangedHandler.Handle(new UnitCoordChanged<T>(fromUnit, to));
      toDict[to] = fromUnit;
    }

    void SwapUnits(Coord from, Coord to, Dictionary<Coord, T> fromDict,
        Dictionary<Coord, T> toDict, T fromUnit) {
      unitCoordChangedHandler.Handle(new UnitCoordChanged<T>(fromUnit, to));
      unitCoordChangedHandler.Handle(new UnitCoordChanged<T>(toDict[to], from));
      fromDict[from] = toDict[to];
      toDict[to] = fromUnit;
    }

    readonly IHandler<UnitCoordChanged<T>> unitCoordChangedHandler;
    readonly Dictionary<Coord, T> benchUnits,
      boardUnits;
  }
}