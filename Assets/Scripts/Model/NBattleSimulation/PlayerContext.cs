using System.Collections.Generic;
using System.Linq;
using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation {
  public class PlayerContext {
    public PlayerContext(Player player1, Player player2) {
      this.player1 = player1;
      this.player2 = player2;
    }
    
    #region Player

    public IEnumerable<Unit> GetBoardUnits(EPlayer player) => 
      Get(player).BoardUnits.Values;

    public bool HasUnits(EPlayer player) =>
      Get(player).BoardUnits.Values.Any(u => u.IsAlive);

    public void MoveUnit(Coord from, Coord to, EPlayer player) => 
      Get(player).MoveUnit(from, to);

    public (bool, Coord) InstantiateToBenchStart(string name, EPlayer player) =>
      Get(player).InstantiateToBenchStart(name, player);

    public (bool, Coord) DestroyFromBenchEnd(EPlayer player) => 
      Get(player).DestroyFromBenchEnd(player);
    
    public void InstantiateToBench(string name, Coord coord, EPlayer player) => 
      Get(player).InstantiateToBench(name, coord, player);

    public void InstantiateToBoard(string name, Coord coord, EPlayer player) => 
      Get(player).InstantiateToBoard(name, coord, player);
    
    public void DestroyAll() {
      Get(EPlayer.First).DestroyAll();
      Get(EPlayer.Second).DestroyAll();
    }

    #endregion

    public Dictionary<Coord, Unit> BoardUnits() {
      var units = new Dictionary<Coord, Unit>();
      
      foreach (var (coord, unit) in player1.BoardUnits) units[coord] = unit;
      foreach (var (coord, unit) in player2.BoardUnits) units[coord] = unit;
      
      return units;
    }

    public Dictionary<Coord, Unit> GetBoardUnitDict(EPlayer player) => Get(player).BoardUnits;
    public Dictionary<Coord, Unit> GetBenchUnitDict(EPlayer player) => Get(player).BenchUnits;

    Player Get(EPlayer player) => player == EPlayer.First ? player1 : player2;
    
    readonly Player player1;
    readonly Player player2;
  }
}