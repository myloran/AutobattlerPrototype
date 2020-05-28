using System.Collections.Generic;
using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation {
  public class Board {
    public readonly Dictionary<Coord, Unit> Units = new Dictionary<Coord, Unit>(20);

    public void Reset(Player player1, Player player2) {
      Units.Clear();
      
      foreach (var unit in player1.BoardUnits) {
        Units[unit.Key] = unit.Value;
      }
      
      foreach (var unit in player2.BoardUnits) {
        Units[unit.Key] = unit.Value;
      }
    }
  }
}