using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Shared.Client.Events;

namespace Model.NAbility.Effects {
  public class SilenceEffect : IEffect {
    public SilenceEffect(IEventBus bus, F32 duration) {
      this.bus = bus;
      this.duration = duration;
    }

    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        unit.ApplySilence(context.CurrentTime + duration);
        bus.Raise(new UpdateSilenceDurationEvent(unit.SilenceEndTime, unit.Coord));
      }
    }

    readonly IEventBus bus;
    readonly F32 duration;
  }
}