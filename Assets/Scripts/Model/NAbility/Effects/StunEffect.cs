using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Shared.Client.Events;

namespace Model.NAbility.Effects {
  public class StunEffect : IEffect {
    public StunEffect(IEventBus bus, F32 duration) {
      this.bus = bus;
      this.duration = duration;
    }

    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        if (!unit.IsAlive) return;

        unit.ApplyStun(context.CurrentTime, duration);
        
        bus.Raise(new UpdateStunDurationEvent(unit.StunEndTime, unit.Coord));
        if (unit.IsMovePaused) bus.Raise(new PauseMoveEvent(unit.Coord));
      }
    }

    readonly IEventBus bus;
    readonly F32 duration;
  }
}