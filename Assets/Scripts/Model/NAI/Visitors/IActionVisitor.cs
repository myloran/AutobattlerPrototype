using Model.NAI.Actions;

namespace Model.NAI.Visitors {
  public interface IActionVisitor {
    void VisitAttackAction(AttackAction action);
    void VisitMoveAction(MoveAction action);
  }
}