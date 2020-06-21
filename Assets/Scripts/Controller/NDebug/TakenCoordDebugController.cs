using Controller.Update;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Poco;
using UnityEngine;
using View.NTile;
using View.Presenters;

namespace Controller.NDebug {
  public class TakenCoordDebugController : ITick {
    public TakenCoordDebugController(Board board, DebugController debugController, 
        TilePresenter tilePresenter) {
      this.board = board;
      this.debugController = debugController;
      this.tilePresenter = tilePresenter;
    }
    
    public void Tick() {
      if (!debugController.Info.IsDebugOn) {
        if (debugController.Info.IsDebugOn != debugController.WasDebugOn) //TODO: simplify 
          ClearHighlight();
        return;
      }

      ClearHighlight();
      
      foreach (var unit in board.Values) {
        var tile = tilePresenter.TileAt(unit.Coord);  
        
        if (unit.TakenCoord != Coord.Invalid) {
          var anotherTile = tilePresenter.TileAt(unit.TakenCoord);
          anotherTile.Debug(TakenCoordColor(unit));
        }
          
        tile.Debug(CoordColor(unit));
      }
      
      Color CoordColor(IUnit unit) => unit.Player == EPlayer.First ? Color.blue : Color.red;
      Color TakenCoordColor(IUnit unit) => unit.Player == EPlayer.First ? Color.cyan : Color.magenta;
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