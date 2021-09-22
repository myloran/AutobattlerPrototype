using System.Collections.Generic;
using System.Linq;
using Model.NAbility.Abstraction;
using Model.NAbility.Effects;
using Model.NAbility.Timings;
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

      ITargetsSelector targetsSelector = new SingleTargetsSelector();

      var effects = new List<IEffect>();
      if (info.Damage > 0) {
        effects.Add(new DamageEffect(bus, ToF32(info.Damage)));
      }
      else if (info.SilenceDuration > 0) {
        effects.Add(new SilenceEffect(bus, ToF32(info.SilenceDuration)));
      }

      ITiming timing = null;
      if (info.Timing == ETiming.Once) {
        timing = new OnceTiming();
      } 
      else if (info.Timing == ETiming.Period) {
        timing = new PeriodTiming(ToF32(info.TimingPeriod), info.TimingCount, ToF32(info.TimingInitialDelay));
      }
      
      var nestedAbilities = info.NestedAbilities.Select(a => Create(unit, a));

      return new Ability(unit, targetSelector, targetsSelector, effects, timing, info.IsTimingOverridden, nestedAbilities);
    }

    readonly Dictionary<string, AbilityInfo> abilities;
    readonly IEventBus bus;
  }
}                      