using System;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;

namespace Model.NAI.Actions {
  public class MoveAction : BaseAction {
    public MoveAction(Unit unit, IEventBus bus) : base(unit, bus) { }

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var movement = Unit.Movement;
      var target = Unit.Target;
      var ai = Unit.Ai;
      
      var vector = target.Unit.Movement.Coord - movement.Coord;
      var newCoord = movement.Coord + vector.Normalized;
      
      if (context.IsTileEmpty(newCoord)) { //shortest path check
        Move(context, movement, vector.IsDiagonal, newCoord, ai);
        return this;
      }

      var needToWaitTarget = context.Board[newCoord] == Unit.Target.Unit && !vector.IsDiagonal;
      if (needToWaitTarget) { //if target on the tile 
        WaitForTargetToMove(context, ai);
        return this;
      }
      
      var (vector1, vector2) = vector.GetClosestCoordsToMove();
      var newCoord1 = movement.Coord + vector1.Normalized;
      
      if (context.IsTileEmpty(newCoord1)) { //short path check
        Move(context, movement, vector.IsDiagonal, newCoord1, ai);
        return this;
      }
      
      var newCoord2 = movement.Coord + vector2.Normalized;
      
      if (context.IsTileEmpty(newCoord2)) { //short path check
        Move(context, movement, vector.IsDiagonal, newCoord2, ai);
        return this;
      }
      //select random side to walk along or issue normal pathfinder request
      //wait for target to come closer
      WaitForTargetToMove(context, ai);
      return this;
    }

    void WaitForTargetToMove(AiContext context, CAi ai) {
      var time = Unit.Target.Unit.Ai.NextDecisionTime;
      var decisionCommand = new MakeDecisionCommand(ai, context, time);
      context.InsertCommand(decisionCommand, time);
    }

    void Move(AiContext context, CMovement movement, bool isDiagonalMove, Coord newCoord, CAi ai) {
      var time = movement.TimeToMove(isDiagonalMove);
      new StartMoveCommand(context.Board, movement, newCoord, context.CurrentTime,
        time, Bus
      ).Execute();
      var moveCommand = new EndMoveCommand(context.Board, movement, newCoord
        , Bus
      );
      context.InsertCommand(moveCommand, time);
      var decisionCommand = new MakeDecisionCommand(ai, context, time);
      context.InsertCommand(decisionCommand, time);
    }
  }
}