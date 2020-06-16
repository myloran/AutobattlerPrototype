using System;
using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.OkwyLogging;

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
      var targetCoord = target.Unit.Movement.Coord;

      var direction = (targetCoord - movement.Coord).Normalized;
      if (Move(context, movement, direction, ai, target)) return true;

      var (direction1, direction2) = direction.GetClosestDirections();
      if (SmartMove(context, movement, direction1, direction2, targetCoord, ai, target)) return true;

      var direction3 = direction1.GetClosestDirection(direction);
      var direction4 = direction2.GetClosestDirection(direction);
      if (SmartMove(context, movement, direction3, direction4, targetCoord, ai, target)) return true;
      
      var direction5 = direction3.GetClosestDirection(direction1);
      var direction6 = direction4.GetClosestDirection(direction2);
      if (SmartMove(context, movement, direction5, direction6, targetCoord, ai, target)) return true;

      return false;
    }

    bool SmartMove(AiContext context, CMovement movement, Coord direction1, Coord direction2, Coord targetCoord, CAi ai,
      CTarget target) {
      var newCoord1 = movement.Coord + direction1;
      var newCoord2 = movement.Coord + direction2;
      var canMove1 = context.IsTileEmpty(newCoord1);
      var canMove2 = context.IsTileEmpty(newCoord2);

      if (canMove1 && canMove2) {
        var (hasPos1, pos1) = CalculateNextMovePosition(context, newCoord1);
        var (hasPos2, pos2) = CalculateNextMovePosition(context, newCoord2);

          var distance1 = CoordExt.SqrDistance(targetCoord, pos1);
          var distance2 = CoordExt.SqrDistance(targetCoord, pos2);

          if (distance1.IsEqualTo(distance2)) {
            if (hasPos1) return PureMove(newCoord2, direction2);
            if (hasPos2) return PureMove(newCoord1, direction1);
            
            return PureMove(newCoord1, direction1);
          }

          return distance1 < distance2 
            ? PureMove(newCoord1, direction1) 
            : PureMove(newCoord2, direction2);
      }

      if (canMove1) return PureMove(newCoord1, direction1);
      if (canMove2) return PureMove(newCoord2, direction2);

      return false;
          
      bool PureMove(Coord newCoord, Coord direction) {
        var time = movement.TimeToMove(direction.IsDiagonal);
        new StartMoveCommand(context.Board, movement, newCoord, context.CurrentTime, time, Bus)
          .Execute();
      
        context.InsertCommand(time, 
          new EndMoveCommand(context.Board, movement, target, newCoord, Bus));
      
        context.InsertCommand(time, new MakeDecisionCommand(ai, context, time));
        return true;
      }
    }

    (bool, Coord) CalculateNextMovePosition(AiContext context, Coord coord) {
      (bool, Coord) res = (false, coord);
      
      var direction = (Unit.Target.Unit.Movement.Coord - coord).Normalized;
      if (TryMove(direction, ref res)) return res;

      var (direction1, direction2) = direction.GetClosestDirections();
      if (TryMove(direction1, ref res)) return res;
      if (TryMove(direction2, ref res)) return res;

      return res;
      
      bool TryMove(Coord moveDirection, ref (bool, Coord) result) {
        var pos = coord + moveDirection;
        if (!context.IsTileEmpty(pos)) return false;
        
        result = (true, pos);
        return true;
      }
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

    static readonly Logger log = MainLog.GetLogger(nameof(MoveAction));
  }
}