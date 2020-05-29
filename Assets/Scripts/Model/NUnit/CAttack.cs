using System;
using Model.NBattleSimulation;
using Shared;

namespace Model.NUnit {
  public class CAttack {
    public float Damage;
    public float Speed;
    public float SqrRange;
    
    public CAttack(CMovement movement, float damage, float speed, float sqrRange) {
      this.movement = movement;
      Damage = damage;
      Speed = speed;
      SqrRange = sqrRange;
    }

    public bool CanAttack => true;
    public TimePoint AttackTime => Math.Abs(Speed) > float.Epsilon 
      ? new TimePoint(1 / Speed) : 999;

    public bool IsWithinAttackRange(CMovement target) => 
      CoordExt.SqrDistance(movement.Coord, target.Coord) < SqrRange;

    public void Attack(CHealth health) {
      health.CalculateDamage(Damage);
    }

    readonly CMovement movement;
  }
}