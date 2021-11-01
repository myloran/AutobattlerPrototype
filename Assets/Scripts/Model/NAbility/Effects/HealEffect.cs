using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Shared.Client.Events;

namespace Model.NAbility.Effects {
  public class HealEffect : IEffect {
    public HealEffect(IEventBus bus, F32 damage) {
      this.damage = damage;
      this.bus = bus;
    }
    
    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        if (!unit.IsAlive) return;
        
        unit.ApplyHeal(damage);
      
        bus.Raise(new UpdateHealthEvent(unit.Health, unit.Coord));
      }
    }

    readonly IEventBus bus;
    readonly F32 damage;
  }
}