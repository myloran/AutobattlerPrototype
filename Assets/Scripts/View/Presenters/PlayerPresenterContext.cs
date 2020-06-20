using System.Collections.Generic;
using Shared;
using View.NUnit;

namespace View.Presenters {
  public class PlayerPresenterContext {
    public PlayerPresenterContext(PlayerPresenter player1, PlayerPresenter player2) {
      this.player1 = player1;
      this.player2 = player2;
    }
    
    #region Player
    public void MoveUnit(Coord from, Coord to, EPlayer player) => 
      Get(player).MoveUnit(from, to);

    public void InstantiateToBench(string name, Coord coord, EPlayer player) => 
      Get(player).InstantiateToBench(name, coord, player);

    public void InstantiateToBoard(string name, Coord coord, EPlayer player) => 
      Get(player).InstantiateToBoard(name, coord, player);

    public void DestroyFromBench(EPlayer player, Coord coord) =>
      Get(player).DestroyFromBench(coord);
    
    public void DestroyAll() {
      Get(EPlayer.First).DestroyAll();
      Get(EPlayer.Second).DestroyAll();
    }

    #endregion

    public Dictionary<Coord, UnitView> BoardUnits() {
      var units = new Dictionary<Coord, UnitView>();
      
      foreach (var (coord, unit) in player1.BoardUnits) units[coord] = unit;
      foreach (var (coord, unit) in player2.BoardUnits) units[coord] = unit;
      
      return units;
    }
    
    PlayerPresenter Get(EPlayer player) => player == EPlayer.First ? player1 : player2;
    
    readonly PlayerPresenter player1, player2;
  }
}