using System.Collections.Generic;
using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation {
  public class PlayerContext {
    public PlayerContext(Player[] players) {
      this.player1Units = players[0].BoardUnits.Units;
      this.player2Units = players[1].BoardUnits.Units;
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
      player == EPlayer.First ? player1Units.Count == 0 : player2Units.Count == 0;

    Dictionary<Coord, Unit> player1Units,
      player2Units;
  }
}