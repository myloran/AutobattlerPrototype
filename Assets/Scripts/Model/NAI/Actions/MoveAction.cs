using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NDecisionTree;
using Model.NUnit;
using PlasticFloor.EventBus;

namespace Model.NAI.Actions {
  public class MoveAction : IDecisionTreeNode {
    public MoveAction(CMovement movement, CTarget target, CAi ai, IEventBus bus) {
      this.movement = movement;
      this.target = target;
      this.ai = ai;
      this.bus = bus;
    }
    
    public IDecisionTreeNode MakeDecision(AiContext context) {
      var vector = target.Unit.Movement.Coord - movement.Coord;
      var isDiagonalMove = vector.X != 0 && vector.Y != 0;
      var newCoord = movement.Coord + vector.Normalized;
      
      if (context.IsTileEmpty(newCoord)) {
        var time = movement.TimeToMove(isDiagonalMove);
        var startMoveCommand = new StartMoveCommand(context.Board, movement, newCoord
// #if Client
          , context.CurrentTime, time, bus
// #endif
        );
        context.AiHeap[context.CurrentTime] = startMoveCommand;
        var moveCommand = new EndMoveCommand(context.Board, movement, newCoord
          , bus
        ); 
        context.AiHeap[context.CurrentTime + time] = moveCommand;
        var decisionCommand = new MakeDecisionCommand(ai, context);
        context.AiHeap[context.CurrentTime + time] = decisionCommand;
      }
      return this;
    }
    
    readonly CMovement movement;
    readonly CTarget target;
    readonly CAi ai;
    readonly IEventBus bus;
  }
}