using Model.NAI.Visitors;
using Model.NBattleSimulation;
using Model.NUnit;
using Shared;

namespace Model.NAI.UnitCommands {
  public class MoveCommand : ICommand {
    public MoveCommand(Board board, CMovement movement, Coord newCoord) {
      this.board = board;
      this.movement = movement;
      this.newCoord = newCoord;
    }

    public void Execute() {
      board.Units.Remove(movement.Coord);
      movement.Coord = newCoord;
      movement.TakenCoord = Coord.Invalid;
    }

    public void Accept(ICommandVisitor visitor) { }

    readonly Board board;
    readonly CMovement movement;
    readonly CHealth health;
    readonly Coord newCoord;
  }
}