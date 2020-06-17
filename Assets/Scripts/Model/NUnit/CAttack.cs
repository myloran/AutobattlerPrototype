using System;
using FixMath;
using Shared;
using static FixMath.F32;

namespace Model.NUnit {
  public class CAttack {
    public F32 Damage;
    public F32 AnimationSpeed;
    public F32 SqrRange;
    
    public CAttack(CMovement movement, F32 damage, F32 speed, F32 sqrRange, 
        F32 animationSpeed) {
      this.movement = movement;
      Damage = damage;
      attackSpeed = speed;
      SqrRange = sqrRange;
      AnimationSpeed = animationSpeed;
    }

    public bool IsAnimationPlayed(F32 currentTime) {
      return lastAttackTime + AnimationSpeed >= currentTime;
    }

    public F32 AttackTime => attackSpeed > 0 
      ? (1 / attackSpeed) 
      : throw new Exception();

    public bool IsWithinAttackRange(CMovement target) => 
      CoordExt.SqrDistance(movement.Coord, target.Coord) <= SqrRange; //TODO: check if coord == coord.Normalized is more performant

    public void StartAttack(F32 startTime) => lastAttackTime = startTime;
    public void EndAttack() => lastAttackTime = ToF32(0);

    readonly CMovement movement;
    F32 attackSpeed;
    F32 lastAttackTime;

    public override string ToString() => $"{nameof(Damage)}: {Damage}, {nameof(AnimationSpeed)}: {AnimationSpeed}, {nameof(attackSpeed)}: {attackSpeed}, {nameof(SqrRange)}: {SqrRange}, {nameof(lastAttackTime)}: {lastAttackTime}";
  }
}