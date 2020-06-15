using Model.NBattleSimulation;
using Model.NUnit;
using Shared;
using UnityEngine;
using View.Presenters;

namespace Controller.NDebug {
  public class TakenCoordDebugController : ITick {
    public TakenCoordDebugController(TilePresenter tilePresenter, Board board) {
      this.tilePresenter = tilePresenter;
      this.board = board;
    }
    
    public void Tick() {
      foreach (var tile in tilePresenter.Values) {
        tile.Unhighlight();
      }
      
      foreach (var unit in board.Values) {
        var tile = tilePresenter.TileAt(unit.Movement.Coord);
        if (unit.Movement.TakenCoord != Coord.Invalid) {
          var anotherTile = tilePresenter.TileAt(unit.Movement.TakenCoord);
          anotherTile.Debug(TakenCoordColor(unit));
        }
          
        tile.Debug(CoordColor(unit));
      }
      
      Color CoordColor(Unit unit) => unit.Player == EPlayer.First ? Color.blue : Color.red;
      Color TakenCoordColor(Unit unit) => unit.Player == EPlayer.First ? Color.cyan : Color.magenta;
    }


    readonly TilePresenter tilePresenter;
    readonly Board board;
  }
}