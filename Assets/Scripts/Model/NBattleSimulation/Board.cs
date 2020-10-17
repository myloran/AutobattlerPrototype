using System.Collections.Generic;
using System.Linq;
using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Exts;
using Shared.Primitives;
using static Shared.Const;

namespace Model.NBattleSimulation {
  public class Board {
    #region Dict

    public IEnumerable<IUnit> Values => units.Values;
    public void AddUnit(Coord coord, IUnit unit) => units[coord] = unit;
    public void RemoveUnit(Coord coord) => units.Remove(coord);
    public bool ContainsUnit(Coord coord) => units.ContainsKey(coord);
    
    public IUnit TryGetUnit(Coord coord) {
      units.TryGetValue(coord, out var unit);
      return unit;
    }

    #endregion
    
    public void SetContext(PlayerContext context) {
      this.context = context;
      units = context.BoardUnits();
    }

    public IEnumerable<IUnit> GetPlayerUnits(EPlayer player) => context.GetBoardUnits(player);
    public bool HasAliveUnits(EPlayer player) => context.HasAliveUnits(player);

    public bool IsSurrounded(Coord coord) {
      for (int x = -1; x <= 1; x++) {
        for (int y = -1; y <= 1; y++) {
          var newCoord = coord + (x, y);
          if (!newCoord.IsInsideBoard()) continue;
          if (!units.ContainsKey(newCoord)) return false;
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
          
          if (units.ContainsKey(newCoord))
            result.Add(units[newCoord]);
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
          units.TryGetValue(coord + diff, out var value);
          if (value != null) result.Add(value);
        }
    }
    
    public IUnit FindClosestUnitTo(Coord coord, EPlayer player) => 
      GetPlayerUnits(player).Where(u => u.IsAlive).MinBy(u => CoordExt.SqrDistance(coord, u.Coord));

    Dictionary<Coord, IUnit> units = new Dictionary<Coord, IUnit>(MaxUnitsOnBoard * 2); //when unit moves it occupies tile thus * 2
    PlayerContext context;
  }
}