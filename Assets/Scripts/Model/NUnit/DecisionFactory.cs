using Model.NAI.Actions;
using Model.NAI.Decisions;
using Model.NDecisionTree;
using PlasticFloor.EventBus;

namespace Model.NUnit {
  public class DecisionFactory {
    public DecisionFactory(EventBus bus) => this.bus = bus;

    public IDecisionTreeNode Create(Unit unit) {
      var attackAction = new AttackAction();
      var moveAction = new MoveAction(unit.Movement, unit.Target, unit.Ai, bus);
      
      var isWithinAttackRangeDecision = new IsWithinAttackRangeDecision(
        attackAction, moveAction, unit.Attack, unit.Target);
      
      var findNearestTargetAction = new FindNearestTargetAction(
        isWithinAttackRangeDecision, unit.Target, unit.Stats);
      
      var decision = new HasTargetDecision(
        isWithinAttackRangeDecision, findNearestTargetAction, unit.Target);
      
      return decision;
    }
    
    readonly EventBus bus;
  }
}