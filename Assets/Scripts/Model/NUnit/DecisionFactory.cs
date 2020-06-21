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
      var doNothing = WithLogging(new DoNothing(unit, bus));
      var findNearestTargetAction = WithLogging(new FindNearestTargetAction(unit, bus));
      var startAttack = WithLogging(new StartAttackAction(unit, bus));
      var attack = WithLogging(new AttackAction(unit, bus));
      var waitFirstEnemyArriving = WithLogging(new WaitFirstEnemyArriving(unit, bus));
      var waitMoveDiff = WithLogging(new WaitDiffBetweenDiagonalAndStraightMove(unit, bus));
      var move = WithLogging(new MoveAction(unit, bus));
      var waitForAlliesToMoveAction = new WaitForAlliesToMoveAction(unit, bus);

      var isSurrounded = WithLogging(new IsSurrounded(
        waitForAlliesToMoveAction, doNothing, unit));

      var canMove = WithLogging(new CanMove(move, isSurrounded, unit));
      
      var isEnemyArrivingToAdjacentTile = new CheckEnemiesArrivingToAdjacentTile(
        canMove, waitFirstEnemyArriving, waitMoveDiff, unit);

      var canStartAttack = WithLogging(new CanStartAttack(
        startAttack, attack, unit));
      
      var isWithinAttackRangeDecision = WithLogging(new IsWithinAttackRange(
        canStartAttack, isEnemyArrivingToAdjacentTile, unit));
      
      var hasTarget = WithLogging(new HasTarget(
        isWithinAttackRangeDecision, findNearestTargetAction, unit));

      var isAlive = WithLogging(new IsAlive(
        hasTarget, doNothing, unit));

      var isPlayerDead = WithLogging(new IsPlayerDead(
        doNothing, isAlive, unit));

      return isPlayerDead;
    }
    
    IDecisionTreeNode WithLogging(IDecisionTreeNode decision) => new LoggingDecorator(decision);
    
    readonly IEventBus bus;
  }
}