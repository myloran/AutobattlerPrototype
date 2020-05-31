using System;
using Model.NBattleSimulation;
using Shared;

namespace Model.NUnit {
  public class CAttack {
    public float Damage;
    public float AnimationSpeed;
    public float AttackSpeed;
    public float SqrRange;
    
    public CAttack(CMovement movement, float damage, float speed, float sqrRange, float animationSpeed) {
      this.movement = movement;
      Damage = damage;
      AttackSpeed = speed;
      SqrRange = sqrRange;
      AnimationSpeed = animationSpeed;
    }

    public bool IsAnimationPlayed(float currentTime) => lastAttackTime + AnimationSpeed >= currentTime;
    
    public TimePoint AttackTime => Math.Abs(AttackSpeed) > float.Epsilon 
      ? new TimePoint(1 / AttackSpeed) : 999;

    public bool IsWithinAttackRange(CMovement target) {
      return CoordExt.SqrDistance(movement.Coord, target.Coord) <= SqrRange;
    }

    public void StartAttack(float startTime) => lastAttackTime = startTime;

    public void Attack(CHealth health) {
      health.TakeDamage(Damage);
    }

    readonly CMovement movement;
    float lastAttackTime;
  }
}