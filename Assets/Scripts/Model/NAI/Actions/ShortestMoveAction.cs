using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;

namespace Model.NAI.Actions {
public class ShortestMoveAction : BaseAction {
    public override EDecision Type { get; } = EDecision.MoveAction;
    public IDecisionTreeNode FindNearestTarget;
    
    public ShortestMoveAction(Unit unit, IEventBus bus) : base(unit, bus) { }

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

      return this;
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
      var decisionCommand = new MakeDecisionCommand(ai, context, time);
      context.InsertCommand(decisionCommand, time);
    }

    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(MoveAction));
  }
}