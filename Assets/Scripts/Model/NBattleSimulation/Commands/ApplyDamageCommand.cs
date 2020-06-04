using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class ApplyDamageCommand : BaseCommand {
    public ApplyDamageCommand(CHealth health, float damage, CMovement movement, 
        DeathCommand deathCommand, IEventBus bus) {
      this.health = health;
      this.damage = damage;
      this.movement = movement;
      this.deathCommand = deathCommand;
      this.bus = bus;
    }

    public override void Execute() {
      health.TakeDamage(damage);
      
      if (!health.IsAlive) 
        deathCommand.Execute();
      
      bus.Raise(new ApplyDamageEvent(health.Health, movement.Coord));
      
      if (!health.IsAlive) 
        bus.Raise(new DeathEvent(movement.Coord));
    }

    readonly CHealth health;
    readonly float damage;
    readonly CMovement movement;
    readonly DeathCommand deathCommand;
    readonly IEventBus bus;
  }
}