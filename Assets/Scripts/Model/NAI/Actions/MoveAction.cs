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
      var ai = Unit.Ai;

      if (MoveOptimally(context)) return this;

      if (context.IsSurrounded(movement.Coord) && !ai.IsWaiting) {
        ai.IsWaiting = true;
        var decisionCommand = new WaitForAlliesToMoveCommand(movement, ai, context);
        context.InsertCommand(0, decisionCommand);
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

    bool MoveOptimally(AiContext context) {
      var movement = Unit.Movement;
      var target = Unit.Target;
      var ai = Unit.Ai;
      
      var direction = (target.Unit.Movement.Coord - movement.Coord).Normalized;
      if (Move(context, movement, direction, ai, target)) return true;

      var (direction1, direction2) = direction.GetClosestDirections();
      if (Move(context, movement, direction1, ai, target)) return true;
      if (Move(context, movement, direction2, ai, target)) return true;

      var direction3 = direction1.GetClosestDirection(direction);
      if (Move(context, movement, direction3, ai, target)) return true;
      
      var direction4 = direction2.GetClosestDirection(direction);
      if (Move(context, movement, direction4, ai, target)) return true;
      
      var direction5 = direction3.GetClosestDirection(direction1);
      if (Move(context, movement, direction5, ai, target)) return true;
      
      var direction6 = direction4.GetClosestDirection(direction2);
      if (Move(context, movement, direction6, ai, target)) return true;

      return false;
    }

    bool Move(AiContext context, CMovement movement, Coord direction, CAi ai, CTarget target) {
      var newCoord = movement.Coord + direction;
      if (!context.IsTileEmpty(newCoord)) return false;
      
      var time = movement.TimeToMove(direction.IsDiagonal);
      new StartMoveCommand(context.Board, movement, newCoord, context.CurrentTime, time, Bus)
        .Execute();
      
      context.InsertCommand(time, 
        new EndMoveCommand(context.Board, movement, target, newCoord, Bus));
      
      context.InsertCommand(time, new MakeDecisionCommand(ai, context, time));
      return true;
    }

    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(MoveAction));
  }
}