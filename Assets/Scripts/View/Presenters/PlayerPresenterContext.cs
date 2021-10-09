using System.Collections.Generic;
using Shared;
using Shared.Exts;
using Shared.Primitives;
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

    public UnitView InstantiateToBoard(string name, Coord coord, EPlayer player) => 
      Get(player).InstantiateToBoard(name, coord, player);

    public void DestroyFromBench(EPlayer player, Coord coord) =>
      Get(player).DestroyFromBench(coord);
    
    public void DestroyAll() {
      Get(EPlayer.First).DestroyAll();
      Get(EPlayer.Second).DestroyAll();
    }

    #endregion

    public Dictionary<Coord, UnitView> BoardUnits() => 
      player1.BoardUnits.With(player2.BoardUnits);
    
    PlayerPresenter Get(EPlayer player) => player == EPlayer.First ? player1 : player2;
    
    readonly PlayerPresenter player1, player2;
  }
}