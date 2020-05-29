using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class EndMoveCommand : ICommand {
    public EndMoveCommand(Board board, CMovement movement, Coord newCoord, IEventBus bus) {
      this.board = board;
      this.movement = movement;
      this.newCoord = newCoord;
      this.bus = bus;
    }

    public void Execute() {
      board.Units.Remove(movement.Coord);
      movement.Coord = newCoord;
      movement.TakenCoord = Coord.Invalid;
      bus.Raise(new EndMoveEvent(movement.Coord, newCoord));
    }

    readonly Board board;
    readonly CMovement movement;
    readonly CHealth health;
    readonly Coord newCoord;
    readonly IEventBus bus;
  }
}