using Model.NAI.Actions;
using Model.NAI.Decisions;
using Model.NAI.NDecisionTree;
using PlasticFloor.EventBus;

namespace Model.NUnit {
  public class DecisionFactory {
    public DecisionFactory(EventBus bus) => this.bus = bus;

    public IDecisionTreeNode Create(Unit unit) {
      var startAttackAction = new StartAttackAction(unit.Attack, unit.Ai);
      var endAttackAction = new EndAttackAction(unit, bus);
      var moveAction = new MoveAction(unit.Movement, unit.Target, unit.Ai, bus);
      
      var isAttackAnimationPlayed = new IsAttackAnimationPlayed(
        startAttackAction, endAttackAction, unit.Attack);
      
      var isWithinAttackRangeDecision = new IsWithinAttackRange(
        isAttackAnimationPlayed, moveAction, unit.Attack, unit.Target);
      
      var findNearestTargetAction = new FindNearestTargetAction(
        isWithinAttackRangeDecision, unit.Target, unit.Stats);
      
      var decision = new HasTarget(
        isWithinAttackRangeDecision, findNearestTargetAction, unit.Target);
      
      return decision;
    }
    
    readonly EventBus bus;
  }
}