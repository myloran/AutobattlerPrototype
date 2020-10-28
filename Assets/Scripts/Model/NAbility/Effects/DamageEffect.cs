using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NAI.Commands;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Shared.Client.Events;

namespace Model.NAbility.Effects {
  public class DamageEffect : IEffect {
    public DamageEffect(IEventBus bus, F32 damage) {
      this.damage = damage;
      this.bus = bus;
    }
    
    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        unit.TakeDamage(damage);
      
        if (!unit.IsAlive) 
          new DeathCommand(unit, context).Execute();
      
        bus.Raise(new UpdateHealthEvent(unit.Health, unit.Coord));
      
        if (!unit.IsAlive) 
          bus.Raise(new DeathEvent(unit.Coord));
      }
    }

    readonly IEventBus bus;
    readonly F32 damage;
  }
}