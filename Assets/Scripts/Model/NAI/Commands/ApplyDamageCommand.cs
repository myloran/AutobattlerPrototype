using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Commands {
  public class ApplyDamageCommand : BaseCommand {
    public ApplyDamageCommand(IUnit unit, AiContext context, IEventBus bus) : base(unit) {
      this.unit = unit;
      this.context = context;
      this.bus = bus;
    }

    public override void Execute() {
      unit.AccumulateMana();
      bus.Raise(new UpdateManaEvent(unit.Mana, unit.Coord));
      
      if (!unit.TargetExists) return;
      
      var target = unit.Target;
      if (!target.IsAlive) return;
      
      target.TakeDamage(unit.CalculateDamage());
      
      if (!target.IsAlive) 
        new DeathCommand(target, context).Execute();
      
      bus.Raise(new UpdateHealthEvent(target.Health, target.Coord));
      
      if (!target.IsAlive) 
        bus.Raise(new DeathEvent(target.Coord));
    }

    readonly IUnit unit;
    readonly AiContext context;
    readonly IEventBus bus;
  }
}