using System;
using MessagePack;

namespace Shared {
  [Serializable]
  [MessagePackObject]
  public class UnitInfo {
    [Key(0)] public string Name;
    [Key(1)] public float Health;
    [Key(2)] public int Armor;
    [Key(3)] public float Damage;
    [Key(4)] public float AttackSpeed;
    [Key(5)] public float AttackRange;
    [Key(6)] public float MoveSpeed;
    [Key(7)] public float AttackAnimationSpeed;
  }
}