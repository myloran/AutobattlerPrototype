using System;
using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;

namespace Model.NAI.Actions {
  public class MoveAction : BaseAction {
    public override EDecision Type { get; } = EDecision.MoveAction;
    public IDecisionTreeNode FindNearestTarget;
    
    public MoveAction(Unit unit, IEventBus bus) : base(unit, bus) { }

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var movement = Unit.Movement;
      var target = Unit.Target;
      var ai = Unit.Ai;
      
      var direction = target.Unit.Movement.Coord - movement.Coord;
      var newCoord = movement.Coord + direction.Normalized;
      if (context.IsTileEmpty(newCoord)) { //shortest path check
        Move(context, movement, direction.IsDiagonal, newCoord, ai, target);
        return this;
      }

      var (direction1, direction2) = direction.GetClosestDirectionsToMove();
      var newCoord1 = movement.Coord + direction1.Normalized;
      if (context.IsTileEmpty(newCoord1)) {
        Move(context, movement, direction1.IsDiagonal, newCoord1, ai, target);
        return this;
      }
      
      var newCoord2 = movement.Coord + direction2.Normalized;
      if (context.IsTileEmpty(newCoord2)) {
        Move(context, movement, direction2.IsDiagonal, newCoord2, ai, target);
        return this;
      }

      var direction3 = direction1.GetClosestDirectionToMove(direction);
      var newCoord3 = movement.Coord + direction3.Normalized;
      if (context.IsTileEmpty(newCoord3)) {
        Move(context, movement, direction3.IsDiagonal, newCoord3, ai, target);
        return this;
      }
      
      var direction4 = direction2.GetClosestDirectionToMove(direction);
      var newCoord4 = movement.Coord + direction4.Normalized;
      if (context.IsTileEmpty(newCoord4)) {
        Move(context, movement, newCoord4.IsDiagonal, newCoord4, ai, target);
        return this;
      }

      if (context.IsSurrounded(movement.Coord) && !ai.IsWaiting) {
        ai.IsWaiting = true;
        var decisionCommand = new WaitForAlliesToMoveCommand(movement, ai, context);
        context.InsertCommand(decisionCommand, 0);
        return this;
      }
                                      
      if (context.IsCyclicDecision) {
        log.Error($"Cyclic reference {nameof(MoveAction)}");
        return this;
      }
      Unit.Target.Clear();
      context.IsCyclicDecision = true;
      return FindNearestTarget.MakeDecision(context);
    }

    void InsertMakeDecision(AiContext context, CAi ai, float time) {
      var decisionCommand = new MakeDecisionCommand(ai, context, time);
      context.InsertCommand(decisionCommand, time);
    }

    void Move(AiContext context, CMovement movement, bool isDiagonalMove, Coord newCoord, 
        CAi ai, CTarget target) {
      var time = movement.TimeToMove(isDiagonalMove);
      new StartMoveCommand(context.Board, movement, newCoord, context.CurrentTime,time, Bus)
        .Execute();
      var moveCommand = new EndMoveCommand(context.Board, movement, target, newCoord, Bus);
      context.InsertCommand(moveCommand, time);
      InsertMakeDecision(context, ai, time);
    }

    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(MoveAction));
  }
}