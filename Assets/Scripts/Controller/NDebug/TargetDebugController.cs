using Controller.Update;
using Model.NBattleSimulation;
using UnityEngine;
using View.NTile;
using View.Presenters;

namespace Controller.NDebug {
  public class TargetDebugController : ITick {
    public TargetDebugController(Board board, TilePresenter tilePresenter, DebugInfo debugInfo) {
      this.board = board;
      this.tilePresenter = tilePresenter;
      this.debugInfo = debugInfo;
    }
    
    public void Tick() {
      if (!debugInfo.IsDebugOn) return;
      
      foreach (var unit in board.Values) {
        if (!unit.TargetExists) continue;
        
        var from = tilePresenter.PositionAt(unit.Coord);
        var to = tilePresenter.PositionAt(unit.Target.Coord);
        Debug.DrawLine(from, to, Color.red);
      }
    }

    readonly Board board;
    readonly TilePresenter tilePresenter;
    readonly DebugInfo debugInfo;
  }
}