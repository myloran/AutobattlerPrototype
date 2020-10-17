using Shared.Primitives;

namespace View.NUnit {
  public class UnitStats {
    public string Name;
    public float Health;
    public float Mana;
    public int Armor;
    public float Damage;
    public float AttackSpeed;
    public float AttackRange;
    public float MoveSpeed;
    public float AttackAnimationHitTime;
    public float AttackAnimationTotalTime;
    public float ManaPerAttack;
    public string AbilityName;

    public UnitStats(UnitInfo info) {
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