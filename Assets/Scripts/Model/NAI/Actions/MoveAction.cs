using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;

namespace Model.NAI.Actions {
  public class MoveAction : BaseAction {
    public MoveAction(Unit unit, IEventBus bus) : base(unit, bus) { }

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var movement = Unit.Movement;
      var target = Unit.Target;
      var ai = Unit.Ai;
        
      var vector = target.Unit.Movement.Coord - movement.Coord;
      var isDiagonalMove = vector.X != 0 && vector.Y != 0;
      var newCoord = movement.Coord + vector.Normalized;
      
      if (context.IsTileEmpty(newCoord)) {
        var time = movement.TimeToMove(isDiagonalMove);
        var startMoveCommand = new StartMoveCommand(context.Board, movement, newCoord
// #if Client
          , context.CurrentTime, time, Bus
// #endif
        );
        context.AiHeap[context.CurrentTime] = startMoveCommand;
        var moveCommand = new EndMoveCommand(context.Board, movement, newCoord
          , Bus
        ); 
        context.InsertCommand(moveCommand, time);
        var decisionCommand = new MakeDecisionCommand(ai, context, time);
        context.InsertCommand(decisionCommand, time);
      }
      else {
        var time = context.CurrentTime - Unit.Target.Unit.Ai.NextDecisionTime;
        var decisionCommand = new MakeDecisionCommand(ai, context, time);
        context.InsertCommand(decisionCommand, time);
      }
      
      return this;
    }
  }
}