using System;
using System.Collections.Generic;
using System.Linq;
using Model.NUnit.Abstraction;
using Shared.Exts;
using Shared.Primitives;
using static Shared.Const;

namespace Model.NBattleSimulation {
  public class Board {
    #region Dict

    public IEnumerable<IUnit> Values => units.Where(u => u != null);
    
    public void AddUnit(Coord coord, IUnit unit) {
      if (!IsCorrectIndex(coord)) {
        throw new Exception($"Index is not correct: {coord}");
      }
      units[Index(coord)] = unit;
    }

    public void RemoveUnit(Coord coord) {
      if (!IsCorrectIndex(coord)) {
        throw new Exception($"Index is not correct: {coord}");
      }
      units[Index(coord)] = null;
    }

    public bool ContainsUnit(Coord coord) => IsCorrectIndex(coord) && units[Index(coord)] != null;
    public IUnit TryGetUnit(Coord coord) => IsCorrectIndex(coord) ? units[Index(coord)] : null;

    #endregion
    
    static bool IsCorrectIndex(Coord coord) => coord.X >= 0 && coord.X < BoardSizeX && coord.Y >= 0 && coord.Y < BoardSizeY;
    int Index(Coord coord) => coord.X * BoardSizeY + coord.Y;

    public void Reset(PlayerContext context) {
      this.context = context;
      units = new IUnit[MaxTilesOnBoard];
      foreach (var unit in context.BoardUnits()) 
        AddUnit(unit.Key, unit.Value);
    }

    public IEnumerable<IUnit> GetPlayerUnits(EPlayer player) => context.GetBoardUnits(player);
    public bool HasAliveUnits(EPlayer player) => context.HasAliveUnits(player);

    public bool IsSurrounded(Coord coord) {
      for (int x = -1; x <= 1; x++) {
        for (int y = -1; y <= 1; y++) {
          var newCoord = coord + (x, y);
          if (!newCoord.IsInsideBoard()) continue;
          if (!ContainsUnit(newCoord)) return false;
        }
      }
      return true;
    }

    public IEnumerable<IUnit> GetSurroundUnits(Coord coord) {
      var result = new List<IUnit>();
      
      for (int x = -1; x <= 1; x++) {
        for (int y = -1; y <= 1; y++) {
          if (x == 0 && y == 0) continue; 
          
          var newCoord = coord + (x, y);
          var unit = TryGetUnit(newCoord);
          if (unit != null) result.Add(unit);
        }
      }
      
      return result;
    }

    public IEnumerable<IUnit> GetAdjacentUnits(Coord coord) {
      var result = new List<IUnit>();
      AddUnit((0, 1));
      AddUnit((0, -1));
      AddUnit((1, 0));
      AddUnit((-1, 0));
      return result;
      
        void AddUnit(Coord diff) {
          var unit = TryGetUnit(coord + diff);
          if (unit != null) result.Add(unit);
        }
    }
    
    public IUnit FindClosestUnitTo(Coord coord, EPlayer player) => 
      GetPlayerUnits(player).Where(u => u.IsAlive).MinBy(u => CoordExt.SqrDistance(coord, u.Coord));

    IUnit[] units = new IUnit[MaxTilesOnBoard];
    // Dictionary<Coord, IUnit> units = new Dictionary<Coord, IUnit>(MaxUnitsOnBoard * 2); //when unit moves it occupies tile thus * 2
    PlayerContext context;
  }
}