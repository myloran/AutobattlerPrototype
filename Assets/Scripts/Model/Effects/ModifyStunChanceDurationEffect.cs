using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Effects {
  public class ModifyStunChanceDurationEffect : IEffect {
    public ModifyStunChanceDurationEffect(IEventBus bus, F32 stunChanceDuration) {
      this.stunChanceDuration = stunChanceDuration;
      this.bus = bus;
    }
    
    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        if (!unit.IsAlive) return;
        
        unit.ModifyStunChanceDuration(stunChanceDuration);
      }
    }

    readonly IEventBus bus;
    readonly F32 stunChanceDuration;
  }
}