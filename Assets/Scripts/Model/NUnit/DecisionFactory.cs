using Model.NAI.Actions;
using Model.NAI.Decisions;
using Model.NDecisionTree;

namespace Model.NUnit {
  public class DecisionFactory {
    public IDecisionTreeNode Create(Unit unit) {
      var attackAction = new AttackAction();
      var moveAction = new MoveAction(unit.Movement, unit.Target, unit.Ai);
      
      var isWithinAttackRangeDecision = new IsWithinAttackRangeDecision(
        attackAction, moveAction, unit.Attack, unit.Target);
      
      var findNearestTargetAction = new FindNearestTargetAction(
        isWithinAttackRangeDecision, unit.Target, unit.Stats);
      
      var decision = new HasTargetDecision(
        isWithinAttackRangeDecision, findNearestTargetAction, unit.Target);
      
      return decision;
    }
  }
}