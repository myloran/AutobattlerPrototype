using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NAbility.Effects;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAbility {
  public class AbilityFactory {
    public AbilityFactory(Dictionary<string, AbilityInfo> abilities, IEventBus bus) {
      this.abilities = abilities;
      this.bus = bus;
    }

    public Ability Create(IUnit unit, string abilityName) {
      var info = abilities[abilityName];

      ITargetSelector targetSelector = null;
      if (info.Target == ETarget.Unit && info.UnitTargetingRule == EUnitTargetingRule.Closest) {
        targetSelector = new ClosestUnitTargetSelector(info, unit);
      }

      ITargetsSelector targetsSelector = null;
      targetsSelector = new SingleTargetsSelector();

      IEffect effect = null;
      if (info.Damage > 0) {
        effect = new DamageEffect(bus, ToF32((int) info.Damage));
      }


      return new Ability(targetSelector, targetsSelector, effect);
    }

    readonly Dictionary<string, AbilityInfo> abilities;
    readonly IEventBus bus;
  }
}                      