using FixMath;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class StartMoveCommand : BaseCommand {
    public StartMoveCommand(Board board, CMovement movement, Coord newCoord,
      F32 startingTime, F32 duration, IEventBus bus) {
      this.board = board;
      this.movement = movement;
      this.newCoord = newCoord;
      this.startingTime = startingTime;
      this.duration = duration;
      this.bus = bus;
    }

    public override void Execute() {
      var unit = board[movement.Coord];
      board.AddUnit(newCoord, unit);
      movement.TakenCoord = newCoord;
// #if Client
      bus.Raise(new StartMoveEvent(movement.Coord, newCoord, startingTime, duration));
// #endif
    }

    readonly Board board;
    readonly CMovement movement;
    readonly Coord newCoord;
    readonly F32 startingTime;
    readonly F32 duration;
    readonly IEventBus bus;
  }
}