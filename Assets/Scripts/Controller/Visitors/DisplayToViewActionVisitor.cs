using Model.NAI.Actions;
using Model.NAI.Visitors;
using View;

namespace Controller.Visitors {
  public class DisplayToViewActionVisitor : IActionVisitor {
    public DisplayToViewActionVisitor(BoardView board) {
      this.board = board;
    }
    
    public void VisitAttackAction(AttackAction action) {
      throw new System.NotImplementedException();
    }

    public void VisitMoveAction(MoveAction action) {
      var m = action.MoveUnitView; 
      board.Move(m.From, m.To, m.Time);
    }

    readonly BoardView board;
  }
}