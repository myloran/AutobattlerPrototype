using Controller.Update;
using Model.NBattleSimulation;
using UnityEngine;
using View.NTile;
using View.Presenters;

namespace Controller.NDebug {
  public class TargetDebugController : ITick {
    public TargetDebugController(Board board, CoordFinder coordFinder, DebugInfo debugInfo) {
      this.board = board;
      this.coordFinder = coordFinder;
      this.debugInfo = debugInfo;
    }
    
    public void Tick() {
      if (!debugInfo.IsDebugOn) return;
      
      foreach (var unit in board.Values) {
        if (!unit.TargetExists) continue;
        
        var from = coordFinder.PositionAt(unit.Coord);
        var to = coordFinder.PositionAt(unit.Target.Coord);
        Debug.DrawLine(from, to, Color.red);
      }
    }

    readonly Board board;
    readonly CoordFinder coordFinder;
    readonly DebugInfo debugInfo;
  }
}