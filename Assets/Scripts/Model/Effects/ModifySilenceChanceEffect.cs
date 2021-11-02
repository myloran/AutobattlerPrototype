using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Effects {
  public class ModifySilenceChanceEffect : IEffect {
    public ModifySilenceChanceEffect(IEventBus bus, F32 chance) {
      this.chance = chance;
      this.bus = bus;
    }
    
    public void Apply(AiContext context, IEnumerable<IUnit> units) {
      foreach (var unit in units) {
        if (!unit.IsAlive) return;
        
        unit.ModifySilenceChance(chance);
      }
    }

    readonly IEventBus bus;
    readonly F32 chance;
  }
}