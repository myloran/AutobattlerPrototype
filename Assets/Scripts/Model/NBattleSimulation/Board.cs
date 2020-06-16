using System.Collections.Generic;
using Model.NUnit;
using Shared;
using Shared.Abstraction;

namespace Model.NBattleSimulation {
  public class Board : BaseBoard<Unit, Player> {
    public Board(IUnitDict<Unit> units, IUnitDict<Unit> player1Units, 
      IUnitDict<Unit> player2Units) : base(units, player1Units, player2Units) { }

    protected override void OnChangeCoord(Coord coord, Unit unit) { }
    
    public bool IsSurrounded(Coord coord) {
      for (int x = -1; x <= 1; x++) {
        for (int y = -1; y <= 1; y++) {
          var newCoord = coord + (x, y);
          if (!newCoord.IsInsideBoard()) continue;
          if (!Units.Has(newCoord)) return false;
        }
      }
      return true;
    }

    public IEnumerable<Unit> GetSurroundUnits(Coord coord) {
      var units = new List<Unit>();
      
      for (int x = -1; x < 2; x++) {
        for (int y = -1; y < 2; y++) {
          if (x == 0 && y == 0) continue; 
          
          var newCoord = coord + (x, y);
          
          if (Units.Has(newCoord))
            units.Add(Units[newCoord]);
        }
      }
      
      return units;
    }

    public IEnumerable<Unit> GetAdjacentUnits(Coord coord) {
      var units = new List<Unit>();
      AddUnit((0, 1));
      AddUnit((0, -1));
      AddUnit((1, 0));
      AddUnit((-1, 0));
      return units;
      
        void AddUnit(Coord diff) {
          Units.Units.TryGetValue(coord + diff, out var value);
          if (value != null) units.Add(value);
        }
    }
  }
}