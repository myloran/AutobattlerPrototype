using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Commands {
  public class ExecuteAttackCommand : BaseCommand {
    public ExecuteAttackCommand(IUnit unit, AiContext context, IEventBus bus) : base(unit) {
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

      if (unit.CalculateStun()) {
        target.ApplyStun(context.CurrentTime, unit.StunChanceDuration);
        
        bus.Raise(new UpdateStunDurationEvent(target.StunEndTime, target.Coord));
        if (unit.IsMovePaused) bus.Raise(new PauseMoveEvent(target.Coord));
      }
      
      if (unit.CalculateSilence()) {
        target.ApplySilence(context.CurrentTime + unit.StunChanceDuration);
        
        bus.Raise(new UpdateSilenceDurationEvent(target.SilenceEndTime, target.Coord));
      }
      
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