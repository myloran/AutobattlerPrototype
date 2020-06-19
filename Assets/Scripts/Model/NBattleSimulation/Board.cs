using System.Collections.Generic;
using Model.NUnit;
using Shared;
using static Shared.Const;

namespace Model.NBattleSimulation {
  public class Board {
    public IEnumerable<Unit> Values => units.Values;
    public void RemoveUnit(Coord coord) => units.Remove(coord);
    public bool ContainsUnitAt(Coord coord) => units.ContainsKey(coord); 
    public void AddUnit(Coord coord, Unit unit) => units[coord] = unit;
    
    public void Reset(PlayerContext context) {
      this.context = context;
      units = context.Units();
    }

    public IEnumerable<Unit> GetPlayerUnits(EPlayer player) => context.GetPlayerUnits(player);
    public bool HasUnits(EPlayer player) => context.HasUnits(player);

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

    public IEnumerable<Unit> GetSurroundUnits(Coord coord) {
      var result = new List<Unit>();
      
      for (int x = -1; x < 2; x++) {
        for (int y = -1; y < 2; y++) {
          if (x == 0 && y == 0) continue; 
          
          var newCoord = coord + (x, y);
          
          if (units.ContainsKey(newCoord))
            result.Add(units[newCoord]);
        }
      }
      
      return result;
    }

    public IEnumerable<Unit> GetAdjacentUnits(Coord coord) {
      var result = new List<Unit>();
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
    
    Dictionary<Coord, Unit> units = new Dictionary<Coord, Unit>(MaxUnitsOnBoard);
    PlayerContext context;
  }
}