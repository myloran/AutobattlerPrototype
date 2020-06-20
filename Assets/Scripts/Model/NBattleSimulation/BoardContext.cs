using System.Collections.Generic;
using System.Linq;
using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation {
  public class BoardContext {
    public BoardContext(Player[] players) {
      player1Units = players[0].BoardUnits.Units;
      player2Units = players[1].BoardUnits.Units;
    }

    public Dictionary<Coord, Unit> Units() {
      var units = new Dictionary<Coord, Unit>();
      
      foreach (var (coord, unit) in player1Units) units[coord] = unit;
      foreach (var (coord, unit) in player2Units) units[coord] = unit;
      
      return units;
    }
    
    public IEnumerable<Unit> GetPlayerUnits(EPlayer player) => 
      player == EPlayer.First ? player1Units.Values : player2Units.Values;

    public bool HasUnits(EPlayer player) =>
      player == EPlayer.First 
        ? player1Units.Values.Any(u => u.IsAlive) 
        : player2Units.Values.Any(u => u.IsAlive);

    readonly Dictionary<Coord, Unit> player1Units,
      player2Units;
  }
}