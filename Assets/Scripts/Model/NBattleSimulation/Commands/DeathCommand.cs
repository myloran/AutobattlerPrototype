using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation.Commands {
  public class DeathCommand : BaseCommand {
    public DeathCommand(CMovement movement, AiContext context) {
      this.movement = movement;
      this.context = context;
    }
    
    public override void Execute() {
      var board = context.Board;
      board.RemoveUnit(movement.Coord);
      
      if (movement.TakenCoord != Coord.Invalid)
        board.RemoveUnit(movement.TakenCoord);
      
      context.CheckBattleIsOver();
    }

    readonly CMovement movement;
    readonly AiContext context;
  }
}