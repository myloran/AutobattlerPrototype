using Model.NBattleSimulation;
using UnityEngine;
using View;

namespace Controller {
  public class UnitDebugController : ITick {
    public UnitDebugController(Board board, BoardView boardView) {
      this.board = board;
      this.boardView = boardView;
    }
    
    public void Tick() {
      foreach (var unit in board.GetUnits()) {
        if (!unit.Target.Exists) continue;
        var from = boardView.TilePosition(unit.Movement.Coord);
        var to = boardView.TilePosition(unit.Target.Unit.Movement.Coord);
        Debug.DrawLine(from, to, Color.red, 0);
      }
    }

    readonly Player[] players;
    readonly Board board;
    readonly BoardView boardView;
  }
}