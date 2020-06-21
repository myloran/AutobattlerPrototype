using Model.NAI;
using Model.NAI.Actions;
using Model.NAI.Decisions;
using Model.NAI.NDecisionTree;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NUnit {
  public class DecisionFactory {
    public DecisionFactory(IEventBus bus) => this.bus = bus;

    public IDecisionTreeNode Create(IUnit unit) { //TODO: replace ienumerable in model to avoid allocations 
      //TODO: Think of mechanism to not queue MakeDecision command and instead subscribe to interested events and make decision when something happens
      var startAttack = WithLogging(new StartAttackAction(unit, bus));
      var attack = WithLogging(new AttackAction(unit, bus));
      var moveAction = new MoveAction(unit, bus);
      var move = WithLogging(moveAction);
      var waitMoveDiff = WithLogging(new WaitMoveDiff(unit, bus)); //TODO: rename to WaitDiffBetweenDiagonalAndStraightMove
      var waitFirstEnemyArriving = WithLogging(new WaitFirstEnemyArriving(unit, bus));
      var nullAction = WithLogging(new NullAction(unit, bus));
      
      var isEnemyArrivingToAdjacentTile = new CheckEnemiesArrivingToAdjacentTile(
        move, waitFirstEnemyArriving, waitMoveDiff, unit);

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