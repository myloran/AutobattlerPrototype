using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Effects {
  public class TauntEffect : IEffect {
    public TauntEffect(IEventBus bus, IUnit tauntTarget, F32 tauntDuration) {
      this.bus = bus;
      this.tauntTarget = tauntTarget;
      this.tauntDuration = tauntDuration;
    }

    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        if (!unit.IsAlive || !tauntTarget.IsAlive) return;
        
        unit.Taunt(tauntTarget, context.CurrentTime + tauntDuration);
        // bus.Raise(new UpdateSilenceDurationEvent(unit.SilenceEndTime, unit.Coord));
      }
    }

    readonly IEventBus bus;
    readonly IUnit tauntTarget;
    readonly F32 tauntDuration;
  }
}