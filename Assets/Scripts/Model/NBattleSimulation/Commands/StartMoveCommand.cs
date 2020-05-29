using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.Events;

namespace Model.NBattleSimulation.Commands {
  public class StartMoveCommand : ICommand {
    public StartMoveCommand(Board board, CMovement movement, Coord newCoord,
        TimePoint startingTime, float duration, IEventBus bus) {
      this.board = board;
      this.movement = movement;
      this.newCoord = newCoord;
      this.startingTime = startingTime;
      this.duration = duration;
      this.bus = bus;
    }

    public void Execute() {
      var units = board.Units;
      units[newCoord] = units[movement.Coord];
      movement.TakenCoord = newCoord;
      bus.Raise(new StartMoveEvent(movement.Coord, newCoord, startingTime, duration));
    }

    readonly Board board;
    readonly CMovement movement;
    readonly Coord newCoord;
    readonly TimePoint startingTime;
    readonly float duration;
    readonly IEventBus bus;
  }
}