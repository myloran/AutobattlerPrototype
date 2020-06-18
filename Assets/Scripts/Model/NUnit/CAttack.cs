using System;
using FixMath;
using Shared;
using static FixMath.F32;

namespace Model.NUnit {
  public class CAttack {
    public F32 Damage;
    public F32 AttackAnimationHitTime;
    F32 sqrRange;
    
    public CAttack(CMovement movement, F32 damage, F32 speed, F32 sqrRange, 
        F32 attackAnimationHitTime, F32 attackAnimationTotalTime) {
      this.movement = movement;
      Damage = damage;
      attackSpeed = speed;
      this.sqrRange = sqrRange;
      AttackAnimationHitTime = attackAnimationHitTime;
      this.attackAnimationTotalTime = attackAnimationTotalTime;
    }

    public void Reset() => lastStartAttackTime = Zero;

    public bool CanStartAttack(F32 currentTime) => 
      lastStartAttackTime + AttackAnimationHitTime < currentTime;

    public F32 TimeToFinishAttackAnimation => attackAnimationTotalTime - AttackAnimationHitTime;

    public F32 AttackSpeedTime => attackSpeed > 0 
      ? (1 / attackSpeed) 
      : throw new Exception();

    public bool IsWithinAttackRange(CMovement target) => 
      CoordExt.SqrDistance(movement.Coord, target.Coord) <= sqrRange; //TODO: check if coord == coord.Normalized is more performant

    public void StartAttack(F32 currentTime) => lastStartAttackTime = currentTime;
    public void EndAttack() => lastStartAttackTime = ToF32(0);

    readonly CMovement movement;
    readonly F32 attackAnimationTotalTime;
    F32 attackSpeed;
    F32 lastStartAttackTime;

    public override string ToString() => $"{nameof(Damage)}: {Damage}, {nameof(AttackAnimationHitTime)}: {AttackAnimationHitTime}, {nameof(attackSpeed)}: {attackSpeed}, {nameof(sqrRange)}: {sqrRange}, {nameof(lastStartAttackTime)}: {lastStartAttackTime}";
  }
}