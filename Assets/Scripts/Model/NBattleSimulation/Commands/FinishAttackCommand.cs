using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class FinishAttackCommand : BaseCommand {
    public FinishAttackCommand(CHealth health, CMovement movement, CAttack attack, IEventBus bus) {
      this.health = health;
      this.movement = movement;
      this.attack = attack;
      this.bus = bus;
    }
    
    public override void Execute() {
      if (!health.IsAlive) return;
      
      attack.EndAttack();
      bus.Raise(new IdleEvent(movement.Coord));
    }

    readonly CHealth health;
    readonly CMovement movement;
    readonly CAttack attack;
    readonly IEventBus bus;
  }
}