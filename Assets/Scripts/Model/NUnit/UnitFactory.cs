using System.Collections.Generic;
using Shared;

namespace Model.NUnit {
  public class UnitFactory {
    public UnitFactory(Dictionary<string, UnitInfo> infos, DecisionFactory decisionFactory) {
      this.infos = infos;
      this.decisionFactory = decisionFactory;
    }
    
    public Unit Create(string name, int playerId) {
      var info = infos[name];
      var movement = new CMovement(info.MoveSpeed);
      var attack = new CAttack(movement, info.Damage, info.AttackSpeed, info.AttackRange * info.AttackRange);
      var health = new CHealth(info.Health, info.Armor);
      
      var unit = new Unit {
        Info = info,
        Health = health,
        Attack = attack,
        Movement = movement,
        Target = new CTarget(movement),
        Ai = new CAi(attack, health),
        Stats = new CStats(1, (EPlayer)playerId)
      };

      unit.Ai.Decision = decisionFactory.Create(unit);

      return unit;
    }

    readonly Dictionary<string, UnitInfo> infos;
    readonly DecisionFactory decisionFactory;
  }
}