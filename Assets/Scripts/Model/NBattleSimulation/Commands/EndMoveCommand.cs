using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation.Commands {
  public class EndMoveCommand : ICommand {
    public EndMoveCommand(Board board, CMovement movement, Coord newCoord) {
      this.board = board;
      this.movement = movement;
      this.newCoord = newCoord;
    }

    public void Execute() {
      board.Units.Remove(movement.Coord);
      movement.Coord = newCoord;
      movement.TakenCoord = Coord.Invalid;
    }

    readonly Board board;
    readonly CMovement movement;
    readonly CHealth health;
    readonly Coord newCoord;
  }
}