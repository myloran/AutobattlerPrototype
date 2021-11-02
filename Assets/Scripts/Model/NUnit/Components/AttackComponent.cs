using System;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Const;
using static Shared.Primitives.CoordExt;

namespace Model.NUnit.Components {
  public class AttackComponent : IAttack {
    public F32 Damage { get; }
    public F32 AttackAnimationHitTime { get; }
    public Property StunChanceDuration { get; private set; }
    public Property SilenceChanceDuration { get; private set; }
    
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
    
    public bool IsRanged => sqrRange > 1;

    public void ModifyCritChance(F32 amount) => critChance.Modify(amount);
    public void ModifyStunChance(F32 amount) => stunChance.Modify(amount);
    public void ModifySilenceChance(F32 amount) => stunChance.Modify(amount);
    public void ModifyStunChanceDuration(F32 amount) => StunChanceDuration.Modify(amount);
    public void ModifySilenceChanceDuration(F32 amount) => StunChanceDuration.Modify(amount);

    public bool CalculateStun() => random.NextF32(100) < stunChance;
    public bool CalculateSilence() => random.NextF32(100) < silenceChance;
    public F32 CalculateDamage() => random.NextF32(100) < critChance ? Damage * critDamageMultiplier : Damage;

    public void Reset() {
      critChance.Reset();
      stunChance.Reset();
      StunChanceDuration.Reset();
      EndAttack();
    }

    public F32 ProjectileTravelTimeTo(IMovement target) {
      var diff = movement.Coord.Diff(target.Coord);
      return (diff.X + diff.Y) * projectileTravelTimePerTile;
    }

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

    public override string ToString() => $"{nameof(Damage)}: {Damage}, {nameof(AttackAnimationHitTime)}: {AttackAnimationHitTime}, {nameof(attackSpeed)}: {attackSpeed}, {nameof(critChance)}: {critChance}, {nameof(stunChance)}: {stunChance}, {nameof(StunChanceDuration)}: {StunChanceDuration}, {nameof(sqrRange)}: {sqrRange}, {nameof(lastStartAttackTime)}: {lastStartAttackTime}";
    
    readonly IMovement movement;
    readonly SystemRandomEmbedded random;
    readonly F32 attackAnimationTotalTime,
      projectileTravelTimePerTile,
      critDamageMultiplier = ToF32(1.5f),
      attackSpeed,
      sqrRange;
    Property critChance = ToF32(15),
      stunChance,
      silenceChance;
    F32 lastStartAttackTime;
  }
}