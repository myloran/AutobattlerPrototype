using FixMath;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class StartMoveCommand : BaseCommand {
    public StartMoveCommand(AiContext context, Unit unit, Coord newCoord,
        F32 duration, IEventBus bus) {
      this.context = context;
      startingTime = context.CurrentTime;
      this.unit = unit;
      this.newCoord = newCoord;
      this.duration = duration;
      this.bus = bus;
    }

    public override void Execute() {
      context.AddUnit(newCoord, unit);
      unit.TakenCoord = newCoord;
      bus.Raise(new StartMoveEvent(unit.Coord, newCoord, startingTime, duration));
    }

    readonly AiContext context;
    readonly Unit unit;
    readonly Coord newCoord;
    readonly F32 duration;
    readonly F32 startingTime;
    readonly IEventBus bus;
  }
}