using Model.NAI.UnitCommands;
using Model.NAI.Visitors;
using Model.NBattleSimulation;
using Model.NDecisionTree;
using Model.NUnit;
using Shared;
using UnityEngine;

namespace Model.NAI.Actions {
  public class MoveAction : IDecisionTreeNode {
    public MoveUnitView MoveUnitView;
    
    public MoveAction(CMovement movement, CTarget target, CAi ai) {
      this.movement = movement;
      this.target = target;
      this.ai = ai;
    }
    
    public IDecisionTreeNode MakeDecision(AiContext context) {
      var vector = target.Unit.Movement.Coord - movement.Coord;
      var isDiagonalMove = vector.X != 0 && vector.Y != 0;
      var newCoord = movement.Coord + vector.Normalized;
      
      if (context.IsTileEmpty(newCoord)) {
        var time = movement.TimeToMove(isDiagonalMove);
        context.Board.Units[newCoord] = context.Board.Units[movement.Coord];
        movement.TakenCoord = newCoord;
        MoveUnitView = new MoveUnitView(movement.Coord, newCoord, time);
        var moveCommand = new MoveCommand(context.Board, movement, newCoord); 
        context.AiHeap[context.CurrentTime + time] = moveCommand;
        var decisionCommand = new MakeDecisionCommand(ai, context);
        context.AiHeap[context.CurrentTime + time] = decisionCommand;
      }
      return this;
    }
    
    public void Accept(IActionVisitor visitor) => visitor.VisitMoveAction(this);

    readonly CMovement movement;
    readonly CTarget target;
    readonly CAi ai;
  }
}