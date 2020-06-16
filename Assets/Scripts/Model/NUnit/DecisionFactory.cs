using Model.NAI.Actions;
using Model.NAI.Decisions;
using Model.NAI.NDecisionTree;
using PlasticFloor.EventBus;

namespace Model.NUnit {
  public class DecisionFactory {
    public DecisionFactory(IEventBus bus) => this.bus = bus;

    public IDecisionTreeNode Create(Unit unit) {
      var startAttack = WithLogging(new StartAttackAction(unit, bus));
      var endAttack = WithLogging(new EndAttackAction(unit, bus));
      var moveAction = new MoveAction(unit, bus);
      var move = WithLogging(moveAction);
      var waitMoveDiff = WithLogging(new WaitMoveDiff(unit, bus));
      var waitFirstEnemyArriving = WithLogging(new WaitFirstEnemyArriving(unit, bus));
      var nullAction = WithLogging(new NullAction(unit, bus));
      
      var isEnemyArrivingToAdjacentTile = new CheckEnemiesArrivingToAdjacentTile(
        move, waitFirstEnemyArriving, waitMoveDiff, unit.Movement, unit.Stats, unit.Target);
      
      var isAttackAnimationPlayed = WithLogging(new IsAttackAnimationPlayed(
        endAttack, startAttack, unit.Attack));
      
      var isWithinAttackRangeDecision = WithLogging(new IsWithinAttackRange(
        isAttackAnimationPlayed, isEnemyArrivingToAdjacentTile, unit.Attack, unit.Target));
      
      var findNearestTargetAction = WithLogging(new FindNearestTargetAction(
        unit, bus, isWithinAttackRangeDecision));
      
      var hasTarget = WithLogging(new HasTarget(
        isWithinAttackRangeDecision, findNearestTargetAction, unit.Target));

      var isAlive = WithLogging(new IsAlive(
        hasTarget, nullAction, unit.Health));

      moveAction.FindNearestTarget = findNearestTargetAction;
      
      return isAlive;
    }
    
    IDecisionTreeNode WithLogging(IDecisionTreeNode decision) => new LoggingDecorator(decision);
    
    readonly IEventBus bus;
  }
}