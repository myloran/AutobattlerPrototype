using System;
using System.Collections.Generic;
using System.Linq;
using Model.NAbility.Abstraction;
using Model.NAbility.Effects;
using Model.NAbility.TilesSelector;
using Model.NAbility.Timings;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAbility {
  public class AbilityFactory {
    public AbilityFactory(Dictionary<string, AbilityInfo> abilities, EffectFactory effectFactory, IEventBus bus) {
      this.abilities = abilities;
      this.effectFactory = effectFactory;
      this.bus = bus;
    }

    public Ability Create(IUnit unit, string abilityName) {
      var info = abilities[abilityName];

      IMainTargetSelector targetSelector = null;
      IAdditionalTargetsSelector targetsSelector = null;
      AlongLineTilesSelector tilesSelector = null;
      
      if (info.UnitTargetingRule == EUnitTargetingRule.Closest)
        targetSelector = new InheritTargetFromTargeting(unit);
      else if (info.UnitTargetingRule == EUnitTargetingRule.MaxAbilityRange)
        targetSelector = new MaxAbilityRangeTargetSelector(unit);
      else if (info.UnitTargetingRule == EUnitTargetingRule.Self)
        targetSelector = new SelfTargetSelector(unit);

      Func<Coord> getAbilityOrigin = () => { //TODO: probably move to runtime and pass context/target from ability
        if (info.AbilityOrigin == EAbilityOrigin.Target) return unit.Target?.Coord ?? Coord.Invalid;
        if (info.AbilityOrigin == EAbilityOrigin.Self) return unit.Coord;
        
        return Coord.Invalid;
      };
      
      if (info.Target == ETarget.Unit) {
        if (info.AdditionalTargets == EAdditionalTargets.None)
          targetsSelector = new SingleTargetsSelector();
        else if (info.AdditionalTargets == EAdditionalTargets.AlongLine)
          targetsSelector = new AlongLineTargetsSelector(unit);
        else if (info.AdditionalTargets == EAdditionalTargets.WithinRadius)
          targetsSelector = new WithinRadiusTargetsSelector(unit, getAbilityOrigin);
      }
      else if (info.Target == ETarget.Tile)
        tilesSelector = new AlongLineTilesSelector(unit);

      var baseEffect = effectFactory.Create(info.EffectInfo);
      var effects = new List<IEffect> {baseEffect};
      if (info.EffectInfo.TauntDuration > 0) effects.Add(new TauntEffect(bus, unit, ToF32(info.EffectInfo.TauntDuration)));
      var finalEffect = new CompositeEffect(effects.ToArray());

      ITiming timing = null;
      if (info.Timing == ETiming.Once)
        timing = new OnceTiming();
      else if (info.Timing == ETiming.Period) 
        timing = new PeriodTiming(ToF32(info.TimingInitialDelay), ToF32(info.TimingPeriod), info.TimingCount);

      var nestedAbilities = info.NestedAbilities.Select(a => Create(unit, a)).ToList();
      var targetPlayer = info.TargetPlayer.GetPlayer(unit.Player);
      
      return new Ability(unit, targetPlayer, targetSelector, targetsSelector, tilesSelector, finalEffect, timing, info.IsTimingOverridden, 
        info.NeedRecalculateTarget, nestedAbilities);
    }

    readonly Dictionary<string, AbilityInfo> abilities;
    readonly EffectFactory effectFactory;
    readonly IEventBus bus;
  }
}                      