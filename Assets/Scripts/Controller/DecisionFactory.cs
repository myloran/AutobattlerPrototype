using System;
using Controller.DecisionTree.Data;
using Controller.DecisionTree.Visitor;
using Model.NAI.Actions;
using Model.NAI.Decisions;
using Model.NAI.NDecisionTree;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Controller {
  public class DecisionFactory : IDecisionTreeFactory {
    public DecisionFactory(IEventBus bus, Func<IDecisionTreeNode, IDecisionTreeNode> log, 
        DecisionTreeCreatorVisitor decisionTreeCreator, DecisionTreeComponent component) {
      this.bus = bus;
      this.log = log;
      this.decisionTreeCreator = decisionTreeCreator;
      this.component = component;
    }

    public IDecisionTreeNode Create(IUnit unit) { //TODO: replace ienumerable in model to avoid allocations 
      //TODO: Think of mechanism to not queue MakeDecision command and instead subscribe to interested events and make decision when something happens
      // var doNothing = log(new DoNothing(unit, bus));
      // var findNearestTargetAction = log(new FindNearestTargetAction(unit, bus));
      // var startAttack = log(new StartAttackAction(unit, bus));
      // var attack = log(new AttackAction(unit, bus));
      // var waitFirstEnemyArriving = log(new WaitFirstEnemyArriving(unit, bus));
      // var waitMoveDiff = log(new WaitDiffBetweenDiagonalAndStraightMove(unit, bus));
      // var move = log(new MoveAction(unit, bus));
      // var waitForAlliesToMoveAction = new WaitForAlliesToMoveAction(unit, bus);
      //
      // var isSurrounded = log(new IsSurrounded(
      //   waitForAlliesToMoveAction, doNothing, unit));
      //
      // var canMove = log(new CanMove(move, isSurrounded, unit));
      //
      // var needToWaitFirstEnemyArriving = new NeedToWaitFirstEnemyArriving(
      //   waitFirstEnemyArriving, waitMoveDiff, unit);
      //
      // var isEnemyArrivingToAdjacentTile = new CheckEnemiesArrivingToAdjacentTile(
      //   canMove, needToWaitFirstEnemyArriving, unit);
      //
      // var canStartAttack = log(new CanStartAttack(
      //   startAttack, attack, unit));
      //
      // var isWithinAttackRangeDecision = log(new IsWithinAttackRange(
      //   canStartAttack, isEnemyArrivingToAdjacentTile, unit));
      //
      // var hasTarget = log(new HasTarget(
      //   isWithinAttackRangeDecision, findNearestTargetAction, unit));
      //
      // var isAlive = log(new IsAlive(
      //   hasTarget, doNothing, unit));
      //
      // var isPlayerDead = log(new IsPlayerDead(
      //   doNothing, isAlive, unit));
      //
      // isPlayerDead.Do(doNothing).Else(isAlive);
      // isAlive.Do(hasTarget).Else(doNothing);
      // hasTarget.Do(isWithinAttackRangeDecision).Else(findNearestTargetAction);
      // isWithinAttackRangeDecision.Do(canStartAttack).Else(isEnemyArrivingToAdjacentTile);
      // isEnemyArrivingToAdjacentTile.Do()
      // canStartAttack.Do(startAttack).Else(attack);
      //
      //   
      //               canStartAttack.Do(
      //                   startAttack).Else(
      //                 attack)).Else(
      //             isEnemyArrivingToAdjacentTile)).Else(
      //         findNearestTargetAction)).OnFalse(
      //     doNothing));
      //
      // return isPlayerDead;
      // return log(new DoNothing());
      return decisionTreeCreator.Create(component, unit, bus, log);
    }
    
    readonly IEventBus bus;
    readonly Func<IDecisionTreeNode, IDecisionTreeNode> log;
    readonly DecisionTreeCreatorVisitor decisionTreeCreator;
    readonly DecisionTreeComponent component;
  }
}