using Controller.Update;
using Model.NBattleSimulation;
using Model.NUnit;
using Shared;
using UnityEngine;
using View.Presenters;

namespace Controller.NDebug {
  public class TakenCoordDebugController : ITick {
    public TakenCoordDebugController(Board board, DebugController debugController, TileSpawner tileSpawner) {
      this.board = board;
      this.debugController = debugController;
      this.tileSpawner = tileSpawner;
    }
    
    public void Tick() {
      if (!debugController.Info.IsDebugOn) {
        if (debugController.Info.IsDebugOn != debugController.WasDebugOn) 
          ClearHighlight();
        return;
      }

      ClearHighlight();
      
      foreach (var unit in board.Values) {
        var tile = tileSpawner.TileAt(unit.Coord);
        if (unit.TakenCoord != Coord.Invalid) {
          var anotherTile = tileSpawner.TileAt(unit.TakenCoord);
          anotherTile.Debug(TakenCoordColor(unit));
        }
          
        tile.Debug(CoordColor(unit));
      }
      
      Color CoordColor(Unit unit) => unit.Player == EPlayer.First ? Color.blue : Color.red;
      Color TakenCoordColor(Unit unit) => unit.Player == EPlayer.First ? Color.cyan : Color.magenta;
    }

    void ClearHighlight() {
      foreach (var tile in tileSpawner.Values) {
        tile.Unhighlight();
      }
    }

    readonly TileSpawner tileSpawner;
    readonly Board board;
    readonly DebugController debugController;
  }
}