using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation.Commands {
  public class DeathCommand : ICommand {
    public DeathCommand(CMovement movement, AiContext context) {
      this.movement = movement;
      this.context = context;
    }
    
    public void Execute() {
      var board = context.Board;
      board.RemoveUnitAt(movement.Coord);
      
      if (movement.TakenCoord != Coord.Invalid)
        board.RemoveUnitAt(movement.TakenCoord);
      
      context.CheckBattleIsOver();
    }

    readonly CMovement movement;
    readonly AiContext context;
  }
}