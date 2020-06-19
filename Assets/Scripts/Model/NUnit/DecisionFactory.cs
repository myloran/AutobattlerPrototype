using Model.NAI.Actions;
using Model.NAI.Decisions;
using Model.NAI.NDecisionTree;
using PlasticFloor.EventBus;

namespace Model.NUnit {
  public class DecisionFactory {
    public DecisionFactory(IEventBus bus) => this.bus = bus;

    public IDecisionTreeNode Create(Unit unit) {
      var startAttack = WithLogging(new StartAttackAction(unit, bus));
      var attack = WithLogging(new AttackAction(unit, bus));
      var moveAction = new MoveAction(unit, bus);
      var move = WithLogging(moveAction);
      var waitMoveDiff = WithLogging(new WaitMoveDiff(unit, bus));
      var waitFirstEnemyArriving = WithLogging(new WaitFirstEnemyArriving(unit, bus));
      var nullAction = WithLogging(new NullAction(unit, bus));
      
      var isEnemyArrivingToAdjacentTile = new CheckEnemiesArrivingToAdjacentTile(
        move, waitFirstEnemyArriving, waitMoveDiff, unit.Movement, unit.Stats, unit);

      var canStartAttack = WithLogging(new CanStartAttack(
        startAttack, attack, unit));
      
      var isWithinAttackRangeDecision = WithLogging(new IsWithinAttackRange(
        canStartAttack, isEnemyArrivingToAdjacentTile, unit));
      
      var findNearestTargetAction = WithLogging(new FindNearestTargetAction(
        unit, bus, isWithinAttackRangeDecision));
      
      var hasTarget = WithLogging(new HasTarget(
        isWithinAttackRangeDecision, findNearestTargetAction, unit));

      var isAlive = WithLogging(new IsAlive(
        hasTarget, nullAction, unit));

      moveAction.FindNearestTarget = findNearestTargetAction;
      
      return isAlive;
    }
    
    IDecisionTreeNode WithLogging(IDecisionTreeNode decision) => new LoggingDecorator(decision);
    
    readonly IEventBus bus;
  }
}