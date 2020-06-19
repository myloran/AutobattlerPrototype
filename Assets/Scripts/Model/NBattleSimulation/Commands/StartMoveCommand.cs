using FixMath;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class StartMoveCommand : BaseCommand {
    public StartMoveCommand(Board board, Unit unit, Coord newCoord,
      F32 startingTime, F32 duration, IEventBus bus) {
      this.board = board;
      this.unit = unit;
      this.newCoord = newCoord;
      this.startingTime = startingTime;
      this.duration = duration;
      this.bus = bus;
    }

    public override void Execute() {
      var unit = board[this.unit.Coord];
      board.AddUnit(newCoord, unit);
      this.unit.TakenCoord = newCoord;
// #if Client
      bus.Raise(new StartMoveEvent(this.unit.Coord, newCoord, startingTime, duration));
// #endif
    }

    readonly Board board;
    readonly Unit unit;
    readonly Coord newCoord;
    readonly F32 startingTime;
    readonly F32 duration;
    readonly IEventBus bus;
  }
}