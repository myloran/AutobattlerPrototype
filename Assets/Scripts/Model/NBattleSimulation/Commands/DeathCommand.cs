using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation.Commands {
  public class DeathCommand : BaseCommand {
    public DeathCommand(CMovement movement, AiContext context) {
      this.movement = movement;
      this.context = context;
    }
    
    public override void Execute() {
      //cancel actions of dead unit?
      //end move immediately if it had start move
      //cancel attack
      
      //notify units which had dead unit as target
      //so that they can remove it and start looking for a new target
      //so that they don't wait for target to end move
      //so that they don't wait to attack target
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