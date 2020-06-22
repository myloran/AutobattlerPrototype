using System;
using Model.NAI;
using Model.NAI.Actions;
using Model.NAI.Decisions;
using Model.NAI.NDecisionTree;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NUnit {
  public class DecisionFactory {
    public DecisionFactory(IEventBus bus, Func<IDecisionTreeNode, IDecisionTreeNode> log) {
      this.bus = bus;
      this.log = log;
    }

    public IDecisionTreeNode Create(IUnit unit) { //TODO: replace ienumerable in model to avoid allocations 
      //TODO: Think of mechanism to not queue MakeDecision command and instead subscribe to interested events and make decision when something happens
      var doNothing = log(new DoNothing(unit, bus));
      var findNearestTargetAction = log(new FindNearestTargetAction(unit, bus));
      var startAttack = log(new StartAttackAction(unit, bus));
      var attack = log(new AttackAction(unit, bus));
      var waitFirstEnemyArriving = log(new WaitFirstEnemyArriving(unit, bus));
      var waitMoveDiff = log(new WaitDiffBetweenDiagonalAndStraightMove(unit, bus));
      var move = log(new MoveAction(unit, bus));
      var waitForAlliesToMoveAction = new WaitForAlliesToMoveAction(unit, bus);

      var isSurrounded = log(new IsSurrounded(
        waitForAlliesToMoveAction, doNothing, unit));

      var canMove = log(new CanMove(move, isSurrounded, unit));
      
      var isEnemyArrivingToAdjacentTile = new CheckEnemiesArrivingToAdjacentTile(
        canMove, waitFirstEnemyArriving, waitMoveDiff, unit);

      var canStartAttack = log(new CanStartAttack(
        startAttack, attack, unit));
      
      var isWithinAttackRangeDecision = log(new IsWithinAttackRange(
        canStartAttack, isEnemyArrivingToAdjacentTile, unit));
      
      var hasTarget = log(new HasTarget(
        isWithinAttackRangeDecision, findNearestTargetAction, unit));

      var isAlive = log(new IsAlive(
        hasTarget, doNothing, unit));

      var isPlayerDead = log(new IsPlayerDead(
        doNothing, isAlive, unit));

      return isPlayerDead;
    }
    
    readonly IEventBus bus;
    readonly Func<IDecisionTreeNode, IDecisionTreeNode> log;
  }
}