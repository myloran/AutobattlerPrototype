using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Effects {
  public class ModifySilenceChanceDurationEffect : IEffect {
    public ModifySilenceChanceDurationEffect(IEventBus bus, F32 duration) {
      this.duration = duration;
      this.bus = bus;
    }
    
    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        if (!unit.IsAlive) return;
        
        unit.ModifySilenceChanceDuration(duration);
      }
    }

    readonly IEventBus bus;
    readonly F32 duration;
  }
}