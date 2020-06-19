using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class FinishAttackCommand : BaseCommand {
    public FinishAttackCommand(Unit unit, IEventBus bus) {
      this.unit = unit;
      this.bus = bus;
    }
    
    public override void Execute() {
      if (!unit.IsAlive) return;
      
      unit.EndAttack();
      bus.Raise(new IdleEvent(unit.Coord));
    }

    readonly Unit unit;
    readonly IEventBus bus;
  }
}