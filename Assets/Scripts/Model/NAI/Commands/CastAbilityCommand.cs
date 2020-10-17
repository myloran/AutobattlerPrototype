using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Commands {
  public class CastAbilityCommand : BaseCommand {
    public CastAbilityCommand(IUnit unit, AiContext context, IEventBus bus) {
      this.unit = unit;
      this.context = context;
      this.bus = bus;
    }

    public override void Execute() {
      if (!unit.TargetExists) return;
      
      var target = unit.Target;
      if (!target.IsAlive) return;

      unit.CastAbility(context);
      bus.Raise(new UpdateManaEvent(unit.Mana, unit.Coord));
    }

    readonly IUnit unit;
    readonly AiContext context;
    readonly IEventBus bus;
  }
}