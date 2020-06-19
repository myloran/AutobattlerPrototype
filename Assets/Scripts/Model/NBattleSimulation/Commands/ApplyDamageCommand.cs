using FixMath;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NBattleSimulation.Commands {
  public class ApplyDamageCommand : BaseCommand {
    public ApplyDamageCommand(Unit unit, DeathCommand deathCommand, IEventBus bus) {
      this.unit = unit;
      this.deathCommand = deathCommand;
      this.bus = bus;
    }

    public override void Execute() {
      var target = unit.Target;
      if (!target.IsAlive) return;
      
      target.TakeDamage(unit.Damage);
      
      if (!target.IsAlive) 
        deathCommand.Execute();
      
      bus.Raise(new ApplyDamageEvent(target.Health, target.Coord));
      
      if (!target.IsAlive) 
        bus.Raise(new DeathEvent(target.Coord));
    }

    readonly Unit unit;
    readonly DeathCommand deathCommand;
    readonly IEventBus bus;
  }
}