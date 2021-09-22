using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared;
using Shared.Primitives;
using Shared.Shared.Client.Events;

namespace Model.NAI.Commands {
  public class FinishMoveCommand : BaseCommand {
    public FinishMoveCommand(AiContext context, IUnit unit, Coord to, IEventBus bus) : base(unit) {
      this.context = context;
      this.unit = unit;
      this.to = to;
      this.bus = bus;
    }

    public override void Execute() {
      var from = unit.Coord;
      context.RemoveUnit(from);
      unit.Coord = to;
      unit.TakenCoord = Coord.Invalid;
      unit.ClearTarget();
      bus.Raise(new FinishMoveEvent(from, to));
    }

    readonly AiContext context;
    readonly IUnit unit;
    readonly Coord to;
    readonly IEventBus bus;
  }
}