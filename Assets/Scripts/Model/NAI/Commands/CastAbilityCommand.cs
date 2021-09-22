using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Commands {
  public class CastAbilityCommand : BaseCommand {
    public CastAbilityCommand(IUnit unit, AiContext context, IEventBus bus) : base(unit) {
      this.unit = unit;
      this.context = context;
      this.bus = bus;
    }

    public override void Execute() {
      if (!unit.TargetExists) return; //TODO: check is probably not required
      
      var target = unit.Target;
      if (!target.IsAlive) return; //TODO: check is probably not required

      unit.CastAbility(context);
      bus.Raise(new UpdateManaEvent(unit.Mana, unit.Coord));
    }

    readonly IUnit unit;
    readonly AiContext context;
    readonly IEventBus bus;
  }
}