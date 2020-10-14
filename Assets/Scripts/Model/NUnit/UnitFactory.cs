using System.Collections.Generic;
using Model.NAbility;
using Model.NUnit.Abstraction;
using Model.NUnit.Components;
using Shared;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NUnit {
  public class UnitFactory {
    public UnitFactory(Dictionary<string, UnitInfo> infos, IDecisionTreeFactory decisionFactory,
        AbilityFactory abilityFactory) {
      this.infos = infos;
      this.decisionFactory = decisionFactory;
      this.abilityFactory = abilityFactory;
    }
    
    public IUnit Create(string name, Coord coord, EPlayer player) {
      var info = infos[name];
      var movement = new MovementComponent(coord, ToF32(info.MoveSpeed));
      var health = new HealthComponent(ToF32(info.Health), ToF32(info.Armor));
      
      var attack = new AttackComponent(movement, ToF32(info.Damage), ToF32(info.AttackSpeed), 
        ToF32(info.AttackRange * info.AttackRange), ToF32(info.AttackAnimationHitTime),
        ToF32(info.AttackAnimationTotalTime));

      var ability = abilityFactory.Create(name);

      var unit = new Unit(health, attack, movement, new TargetComponent(movement), 
        new AiComponent(), new StatsComponent(name, 1, player), ability);

      unit.SetDecisionTree(decisionFactory.Create(unit));

      return unit;
    }

    readonly Dictionary<string, UnitInfo> infos;
    readonly IDecisionTreeFactory decisionFactory;
    readonly AbilityFactory abilityFactory;
  }
}