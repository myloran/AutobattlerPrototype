using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;
using Shared.Shared.Client.Events;

namespace Model.NAI.Commands {
  public class StartMoveCommand : BaseCommand {
    public StartMoveCommand(AiContext context, IUnit unit, Coord newCoord,
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
    readonly IUnit unit;
    readonly Coord newCoord;
    readonly F32 duration;
    readonly F32 startingTime;
    readonly IEventBus bus;
  }
}