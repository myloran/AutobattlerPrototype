using System;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Const;
using static Shared.Primitives.CoordExt;

namespace Model.NUnit.Components {
  public class AttackComponent : IAttack {
    public F32 Damage { get; }
    public F32 AttackAnimationHitTime { get; }
    
    public AttackComponent(IMovement movement, SystemRandomEmbedded random, F32 damage, F32 speed, F32 sqrRange,
      F32 attackAnimationHitTime, F32 attackAnimationTotalTime, F32 projectileTravelTimePerTile) {
      this.movement = movement;
      this.random = random;
      this.sqrRange = sqrRange;
      this.attackAnimationTotalTime = attackAnimationTotalTime;
      this.projectileTravelTimePerTile = projectileTravelTimePerTile;
      Damage = damage;
      attackSpeed = speed;
      AttackAnimationHitTime = attackAnimationHitTime;
    }

    public F32 CalculateDamage() => random.Next(100) < CritChance ? Damage * critDamageBonus : Damage;

    public void Reset() => EndAttack();
    
    public F32 ProjectileTravelTimeTo(IMovement target) {
      var diff = movement.Coord.Diff(target.Coord);
      return (diff.X + diff.Y) * projectileTravelTimePerTile;
    }

    public bool IsRanged => sqrRange > 1;

    public bool CanStartAttack(F32 currentTime) => 
      lastStartAttackTime + AttackAnimationHitTime < currentTime;

    public F32 TimeToFinishAttackAnimation => attackAnimationTotalTime - AttackAnimationHitTime;

    public F32 AttackSpeedTime => attackSpeed > 0 
      ? (1 / attackSpeed) 
      : throw new Exception();

    public bool IsWithinAttackRange(IMovement target) => 
      SqrDistance(movement.Coord, target.Coord) <= sqrRange; //TODO: check if coord == coord.Normalized is more performant

    public void StartAttack(F32 currentTime) => lastStartAttackTime = currentTime;
    public void EndAttack() => lastStartAttackTime = -MaxBattleDuration;

    public override string ToString() => $"{nameof(Damage)}: {Damage}, {nameof(AttackAnimationHitTime)}: {AttackAnimationHitTime}, {nameof(attackSpeed)}: {attackSpeed}, {nameof(sqrRange)}: {sqrRange}, {nameof(lastStartAttackTime)}: {lastStartAttackTime}";
    
    readonly IMovement movement;
    readonly SystemRandomEmbedded random;
    readonly F32 attackAnimationTotalTime;
    readonly F32 projectileTravelTimePerTile;
    readonly F32 critDamageBonus = F32.ToF32(1.5f);
    const int CritChance = 15;
    F32 attackSpeed;
    F32 lastStartAttackTime;
    F32 sqrRange;
  }
}