using System;
using System.Collections.Generic;

namespace Shared.Primitives {
  [Serializable]
  public class UnitInfo : IInfo {
    public string Name { get; set; }
    public float Health;
    public int Armor;
    public float Damage;
    public float AttackSpeed;
    public float AttackRange;
    public float ProjectileTravelTimePerTile;
    public float MoveSpeed;
    public float AttackAnimationHitTime;
    public float AttackAnimationTotalTime;
    public float ManaPerAttack;
    public string AbilityName;
    public List<string> SynergyNames = new List<string>();

    public UnitInfo() { }

    public UnitInfo(UnitInfo info) {
      Name = info.Name;
      Health = info.Health;
      Armor = info.Armor;
      Damage = info.Damage;
      AttackSpeed = info.AttackSpeed;
      AttackRange = info.AttackRange;
      ProjectileTravelTimePerTile = info.ProjectileTravelTimePerTile;
      MoveSpeed = info.MoveSpeed;
      AttackAnimationHitTime = info.AttackAnimationHitTime;
      AttackAnimationTotalTime = info.AttackAnimationTotalTime;
      ManaPerAttack = info.ManaPerAttack;
    }
  }
}