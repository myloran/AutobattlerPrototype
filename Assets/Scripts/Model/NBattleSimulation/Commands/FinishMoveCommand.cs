using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class FinishMoveCommand : BaseCommand {
    public FinishMoveCommand(AiContext context, IUnit unit, Coord newCoord, IEventBus bus) {
      this.context = context;
      this.unit = unit;
      this.newCoord = newCoord;
      this.bus = bus;
    }

    public override void Execute() {
      var fromCoord = unit.Coord;
      context.RemoveUnit(unit.Coord);
      unit.Coord = newCoord;
      unit.TakenCoord = Coord.Invalid;
      unit.ClearTarget();
      bus.Raise(new FinishMoveEvent(fromCoord, newCoord));
    }

    readonly AiContext context;
    readonly IUnit unit;
    readonly Coord newCoord;
    readonly IEventBus bus;
  }
}