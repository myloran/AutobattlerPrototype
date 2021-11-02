using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Effects {
  public class ModifyCritChanceEffect : IEffect {
    public ModifyCritChanceEffect(IEventBus bus, F32 critChance) {
      this.critChance = critChance;
      this.bus = bus;
    }
    
    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        if (!unit.IsAlive) return;
        
        unit.ModifyCritChance(critChance);
      }
    }

    readonly IEventBus bus;
    readonly F32 critChance;
  }
}