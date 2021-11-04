using System;
using System.Collections.Generic;
using Model.NAbility;
using Model.NUnit.Abstraction;
using Model.NUnit.Components;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NUnit {
  public class UnitFactory {
    public UnitFactory(Dictionary<string, UnitInfo> infos, IDecisionTreeFactory decisionFactory,
        Dictionary<string, AbilityInfo> abilities, AbilityFactory abilityFactory, SystemRandomEmbedded random) {
      this.infos = infos;
      this.decisionFactory = decisionFactory;
      this.abilities = abilities;
      this.abilityFactory = abilityFactory;
      this.random = random;
    }
    
    public IUnit Create(string name, Coord coord, EPlayer player) {
      var info = infos[name];
      ClampUnitValues(info);
      
      var abilityInfo = abilities[info.AbilityName];
      ClampAbilityValues(abilityInfo);
      
      var movement = new MovementComponent(coord, ToF32(info.MoveSpeed));
      var health = new HealthComponent(ToF32(info.Health), ToF32(info.Armor));
      
      var attack = new AttackComponent(movement, random, ToF32(info.Damage), ToF32(info.AttackSpeed), 
        ToF32(info.AttackRange * info.AttackRange), ToF32(info.AttackAnimationHitTime),
        ToF32(info.AttackAnimationTotalTime), ToF32(info.ProjectileTravelTimePerTile));
      

      var ability = new AbilityComponent(movement, ToF32(abilityInfo.AbilityRadius),
        ToF32(abilityInfo.TargetingRange), ToF32(info.ManaPerAttack), ToF32(abilityInfo.AnimationHitTime), 
        ToF32(abilityInfo.AnimationTotalTime));

      var ai = new AiComponent();
      var stun = new StunComponent(attack, ability, ai, movement);

      var stats = new StatsComponent(name, 1, player, info.SynergyNames);
      var unit = new Unit(health, attack, movement, new TargetingComponent(), ai, 
        stats, ability, new SilenceComponent(), stun);

      unit.SetDecisionTree(decisionFactory.Create(unit));
      unit.SetAbility(abilityFactory.Create(unit, info.AbilityName));

      return unit;
    }

    void ClampUnitValues(UnitInfo info) {
      info.ProjectileTravelTimePerTile = Math.Max(0.01f, info.ProjectileTravelTimePerTile);
    }

    void ClampAbilityValues(AbilityInfo abilityInfo) {
      abilityInfo.TargetingRange = Math.Max(1, abilityInfo.TargetingRange);
      abilityInfo.AbilityRadius = Math.Max(1, abilityInfo.AbilityRadius);
      abilityInfo.TimingInitialDelay = Math.Max(0, abilityInfo.TimingInitialDelay);
    }

    readonly Dictionary<string, UnitInfo> infos;
    readonly IDecisionTreeFactory decisionFactory;
    readonly Dictionary<string, AbilityInfo> abilities;
    readonly AbilityFactory abilityFactory;
    readonly SystemRandomEmbedded random;
  }
}