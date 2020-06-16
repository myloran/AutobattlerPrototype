using Controller.Update;
using Model.NBattleSimulation;
using UnityEngine;
using View.Presenters;

namespace Controller.NDebug {
  public class TargetDebugController : ITick {
    public TargetDebugController(Board board, TilePresenter tilePresenter) {
      this.board = board;
      this.tilePresenter = tilePresenter;
    }
    
    public void Tick() {
      foreach (var unit in board.Values) {
        if (!unit.Target.Exists) continue;
        var from = tilePresenter.PositionAt(unit.Movement.Coord);
        var to = tilePresenter.PositionAt(unit.Target.Unit.Movement.Coord);
        Debug.DrawLine(from, to, Color.red);
      }
    }

    readonly Board board;
    readonly TilePresenter tilePresenter;
  }
}