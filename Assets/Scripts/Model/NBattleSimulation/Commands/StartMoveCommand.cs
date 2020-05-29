using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.Events;

namespace Model.NBattleSimulation.Commands {
  public class StartMoveCommand : ICommand {
    public StartMoveCommand(Board board, CMovement movement, Coord newCoord, 
        IEventBus bus) {
      this.board = board;
      this.movement = movement;
      this.newCoord = newCoord;
      this.bus = bus;
    }

    public void Execute() {
      var units = board.Units;
      units[newCoord] = units[movement.Coord];
      movement.TakenCoord = newCoord;
      bus.Raise(new StartMoveEvent(movement.Coord, newCoord));
    }

    readonly Board board;
    readonly CMovement movement;
    readonly Coord newCoord;
    readonly IEventBus bus;
  }
}