using Controller.Update;
using Model.NBattleSimulation;
using Model.NUnit;
using Shared;
using UnityEngine;
using View.Presenters;

namespace Controller.NDebug {
  public class TakenCoordDebugController : ITick {
    public TakenCoordDebugController(TilePresenter tilePresenter, Board board, DebugController debugController) {
      this.tilePresenter = tilePresenter;
      this.board = board;
      this.debugController = debugController;
    }
    
    public void Tick() {
      if (!debugController.Info.IsDebugOn) {
        if (debugController.Info.IsDebugOn != debugController.WasDebugOn) 
          ClearHighlight();
        return;
      }

      ClearHighlight();
      
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

    void ClearHighlight() {
      foreach (var tile in tilePresenter.Values) {
        tile.Unhighlight();
      }
    }

    readonly TilePresenter tilePresenter;
    readonly Board board;
    readonly DebugController debugController;
  }
}