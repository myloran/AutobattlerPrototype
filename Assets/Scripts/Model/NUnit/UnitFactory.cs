using System.Collections.Generic;
using Shared;
using static FixMath.F32;

namespace Model.NUnit {
  public class UnitFactory {
    public UnitFactory(Dictionary<string, UnitInfo> infos, DecisionFactory decisionFactory) {
      this.infos = infos;
      this.decisionFactory = decisionFactory;
    }
    
    public Unit Create(string name, Coord coord, EPlayer player) {
      var info = infos[name];
      var movement = new CMovement(coord, ToF32(info.MoveSpeed));
      var attack = new CAttack(movement, ToF32(info.Damage), ToF32(info.AttackSpeed), 
        ToF32(info.AttackRange * info.AttackRange), ToF32(info.AttackAnimationHitTime),
        ToF32(info.AttackAnimationTotalTime));
      var health = new CHealth(ToF32(info.Health), ToF32(info.Armor));

      var unit = new Unit(health, attack, movement, new CTarget(movement), new CAi(),
        new CStats(name, 1, player));

      unit.SetDecisionTree(decisionFactory.Create(unit));

      return unit;
    }

    readonly Dictionary<string, UnitInfo> infos;
    readonly DecisionFactory decisionFactory;
  }
}