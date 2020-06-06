using Model.NBattleSimulation;
using Model.NUnit;
using Shared;

namespace Model {
  public class ModelContext {
    public ModelContext(Player[] players) {
      this.players = players;
    }
    
    public Unit GetUnit(Coord coord) {
      if (coord.Y == -1) return players[0].BenchUnits[coord];
      if (coord.Y == -2) return players[1].BenchUnits[coord];

      return players[0].BoardUnits.Contains(coord) 
        ? players[0].BoardUnits[coord] 
        : players[1].BoardUnits[coord];
    }

    readonly Player[] players;
  }
}