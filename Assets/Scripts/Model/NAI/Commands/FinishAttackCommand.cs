using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Commands {
  public class FinishAttackCommand : BaseCommand {
    public FinishAttackCommand(IUnit unit, IEventBus bus) : base(unit) {
      this.unit = unit;
      this.bus = bus;
    }
    
    public override void Execute() {
      unit.EndAttack();
      if (!unit.IsAlive) return;
      
      bus.Raise(new IdleEvent(unit.Coord));
    }

    readonly IUnit unit;
    readonly IEventBus bus;
  }
}