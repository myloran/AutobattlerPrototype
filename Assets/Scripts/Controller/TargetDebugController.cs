using Model.NBattleSimulation;
using UnityEngine;
using View.Presenters;

namespace Controller {
  public class TargetDebugController : ITick {
    public TargetDebugController(Board board, TilePresenter tilePresenter) {
      this.board = board;
      this.tilePresenter = tilePresenter;
    }
    
    public void Tick() {
      foreach (var unit in board.GetUnits()) {
        if (!unit.Target.Exists) continue;
        var from = tilePresenter.PositionAt(unit.Movement.Coord);
        var to = tilePresenter.PositionAt(unit.Target.Unit.Movement.Coord);
        Debug.DrawLine(from, to, Color.red, 0);
      }

    }

    readonly Board board;
    readonly TilePresenter tilePresenter;
  }
}