using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class FinishMoveCommand : BaseCommand {
    public FinishMoveCommand(Board board, Unit unit, Coord newCoord, IEventBus bus) {
      this.board = board;
      this.unit = unit;
      this.newCoord = newCoord;
      this.bus = bus;
    }

    public override void Execute() {
      var fromCoord = unit.Coord;
      board.RemoveUnit(unit.Coord);
      unit.Coord = newCoord;
      unit.TakenCoord = Coord.Invalid;
      unit.ClearTarget();
      bus.Raise(new FinishMoveEvent(fromCoord, newCoord));
    }

    readonly Board board;
    readonly Unit unit;
    readonly CHealth health;
    readonly Coord newCoord;
    readonly IEventBus bus;
  }
}