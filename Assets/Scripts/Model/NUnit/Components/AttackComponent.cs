using System;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Primitives.CoordExt;

namespace Model.NUnit.Components {
  public class AttackComponent : IAttack {
    public F32 Damage { get; }
    public F32 AttackAnimationHitTime { get; }
    
    public AttackComponent(IMovement movement, F32 damage, F32 speed, F32 sqrRange, 
        F32 attackAnimationHitTime, F32 attackAnimationTotalTime) {
      this.movement = movement;
      this.sqrRange = sqrRange;
      this.attackAnimationTotalTime = attackAnimationTotalTime;
      Damage = damage;
      attackSpeed = speed;
      AttackAnimationHitTime = attackAnimationHitTime;
    }

    public void Reset() => lastStartAttackTime = Zero;

    public bool CanStartAttack(F32 currentTime) => 
      lastStartAttackTime + AttackAnimationHitTime < currentTime;

    public F32 TimeToFinishAttackAnimation => attackAnimationTotalTime - AttackAnimationHitTime;

    public F32 AttackSpeedTime => attackSpeed > 0 
      ? (1 / attackSpeed) 
      : throw new Exception();

    public bool IsWithinAttackRange(IMovement target) => 
      SqrDistance(movement.Coord, target.Coord) <= sqrRange; //TODO: check if coord == coord.Normalized is more performant

    public void StartAttack(F32 currentTime) => lastStartAttackTime = currentTime;
    public void EndAttack() => lastStartAttackTime = ToF32(0);

    public override string ToString() => $"{nameof(Damage)}: {Damage}, {nameof(AttackAnimationHitTime)}: {AttackAnimationHitTime}, {nameof(attackSpeed)}: {attackSpeed}, {nameof(sqrRange)}: {sqrRange}, {nameof(lastStartAttackTime)}: {lastStartAttackTime}";
    
    readonly IMovement movement;
    readonly F32 attackAnimationTotalTime;
    F32 attackSpeed;
    F32 lastStartAttackTime;
    F32 sqrRange;
  }
}