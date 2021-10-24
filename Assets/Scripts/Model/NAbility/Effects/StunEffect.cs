using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Shared.Client.Events;
using static Shared.Const;

namespace Model.NAbility.Effects {
  public class StunEffect : IEffect {
    public StunEffect(IEventBus bus, F32 duration) {
      this.bus = bus;
      this.duration = duration;
    }

    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        if (!unit.IsAlive) return;

        var oldStunEndTime = unit.StunEndTime == -MaxBattleDuration ? F32.Zero : unit.StunEndTime;
        unit.ApplyStun(context.CurrentTime, duration);
        
        bus.Raise(new UpdateStunDurationEvent(unit.StunEndTime, unit.Coord));
        if (unit.IsMovePaused) {
          var movementAlreadyDone = unit.NextMove.Time - unit.MovementTimeLeft; 
          var stunDurationLeft = unit.StunEndTime - oldStunEndTime;
          var pauseDuration = stunDurationLeft - movementAlreadyDone;
          bus.Raise(new PauseMoveEvent(pauseDuration, unit.Coord));
        }
      }
    }

    readonly IEventBus bus;
    readonly F32 duration;
  }
}