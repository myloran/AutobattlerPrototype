using System.Collections.Generic;
using System.Linq;
using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Exts;
using Shared.Primitives;

namespace Model.NBattleSimulation {
  public class PlayerContext {
    public Dictionary<Coord, IUnit> GetBoardUnitDict(EPlayer player) => Get(player).BoardUnits;
    public Dictionary<Coord, IUnit> GetBenchUnitDict(EPlayer player) => Get(player).BenchUnits;
    
    public PlayerContext(Player player1, Player player2) {
      this.player1 = player1;
      this.player2 = player2;
    }                                                          
    
    #region Player

    public IEnumerable<IUnit> GetBoardUnits(EPlayer player) => 
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

    public IUnit GetUnit(Coord coord) {
      var (hasUnit1, unit1) = Get(EPlayer.First).GetUnit(coord);
      if (hasUnit1) return unit1;
      var (hasUnit2, unit2) = Get(EPlayer.Second).GetUnit(coord);
      if (hasUnit2) return unit2;
      return default;
    }

    public Dictionary<Coord, IUnit> BoardUnits() => player1.BoardUnits.With(player2.BoardUnits);
    Player Get(EPlayer player) => player == EPlayer.First ? player1 : player2;
    
    readonly Player player1;
    readonly Player player2;
  }
}