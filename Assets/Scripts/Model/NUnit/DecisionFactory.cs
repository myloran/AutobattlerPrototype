using Model.NAI.Actions;
using Model.NAI.Decisions;
using Model.NAI.NDecisionTree;
using PlasticFloor.EventBus;

namespace Model.NUnit {
  public class DecisionFactory {
    public DecisionFactory(EventBus bus) => this.bus = bus;

    public IDecisionTreeNode Create(Unit unit) {
      var startAttackAction = WithLogging(new StartAttackAction(unit, bus));
      var endAttackAction = WithLogging(new EndAttackAction(unit, bus));
      var moveAction = WithLogging(new MoveAction(unit, bus));
      var nullAction = WithLogging(new NullAction(unit, bus));
      
      var isAttackAnimationPlayed = WithLogging(new IsAttackAnimationPlayed(
        endAttackAction, startAttackAction, unit.Attack));
      
      var isWithinAttackRangeDecision = WithLogging(new IsWithinAttackRange(
        isAttackAnimationPlayed, moveAction, unit.Attack, unit.Target));
      
      var findNearestTargetAction = WithLogging(new FindNearestTargetAction(
        unit, bus, isWithinAttackRangeDecision));
      
      var hasTarget = WithLogging(new HasTarget(
        isWithinAttackRangeDecision, findNearestTargetAction, unit.Target));

      var isAlive = WithLogging(new IsAlive(
        hasTarget, nullAction, unit.Health));
      
      return hasTarget;
    }
    
    IDecisionTreeNode WithLogging(IDecisionTreeNode decision) => new LoggingDecorator(decision);
    
    readonly EventBus bus;
  }
}