using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class FinishAttackCommand : BaseCommand {
    public FinishAttackCommand(Unit unit, CMovement movement, IEventBus bus) {
      this.unit = unit;
      this.movement = movement;
      this.bus = bus;
    }
    
    public override void Execute() {
      if (!unit.IsAlive) return;
      
      unit.EndAttack();
      bus.Raise(new IdleEvent(movement.Coord));
    }

    readonly Unit unit;
    readonly CMovement movement;
    readonly IEventBus bus;
  }
}