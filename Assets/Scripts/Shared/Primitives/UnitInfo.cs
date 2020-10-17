using System;

namespace Shared.Primitives {
  [Serializable]
  public class UnitInfo {
    public string Name;
    public float Health;
    public int Armor;
    public float Damage;
    public float AttackSpeed;
    public float AttackRange;
    public float MoveSpeed;
    public float AttackAnimationHitTime;
    public float AttackAnimationTotalTime;
    public float ManaPerAttack;
    public string AbilityName;

    public UnitInfo() { }

    public UnitInfo(UnitInfo info) {
      Name = info.Name;
      Health = info.Health;
      Armor = info.Armor;
      Damage = info.Damage;
      AttackSpeed = info.AttackSpeed;
      AttackRange = info.AttackRange;
      MoveSpeed = info.MoveSpeed;
      AttackAnimationHitTime = info.AttackAnimationHitTime;
      AttackAnimationTotalTime = info.AttackAnimationTotalTime;
      ManaPerAttack = info.ManaPerAttack;
    }
  }
}