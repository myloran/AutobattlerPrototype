using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Effects {
  public class ModifyStunChanceEffect : IEffect {
    public ModifyStunChanceEffect(IEventBus bus, F32 stunChance) {
      this.stunChance = stunChance;
      this.bus = bus;
    }
    
    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        if (!unit.IsAlive) return;
        
        unit.ModifyStunChance(stunChance);
      }
    }

    readonly IEventBus bus;
    readonly F32 stunChance;
  }
}