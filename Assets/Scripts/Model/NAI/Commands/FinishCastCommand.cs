using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Commands {
  public class FinishCastCommand : BaseCommand {
    public FinishCastCommand(IUnit unit, IEventBus bus) : base(unit) {
      this.unit = unit;
      this.bus = bus;
    }
    
    public override void Execute() {
      if (!unit.IsAlive) return;
      
      unit.EndCasting();
      bus.Raise(new IdleEvent(unit.Coord));
    }

    readonly IUnit unit;
    readonly IEventBus bus;
  }
}