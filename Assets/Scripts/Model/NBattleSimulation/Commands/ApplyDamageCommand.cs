using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class ApplyDamageCommand : ICommand {
    public ApplyDamageCommand(CHealth health, float damage, CMovement movement, IEventBus bus) {
      this.health = health;
      this.damage = damage;
      this.movement = movement;
      this.bus = bus;
    }

    public void Execute() {
      health.TakeDamage(damage);
      bus.Raise(new ApplyDamageEvent(health.Health, movement.Coord));
    }

    readonly CHealth health;
    readonly float damage;
    readonly CMovement movement;
    readonly IEventBus bus;
  }
}