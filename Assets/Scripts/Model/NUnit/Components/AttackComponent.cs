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
    public F32 StunChanceDuration { get; private set; }
    
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
    

    public void ModifyCritChance(F32 amount) {
      critChance += amount;
      critChance = Clamp(critChance, Zero, maxChance); // refactor if crit chance needs to be restored to a previous value
    }
    
    public void ModifyStunChance(F32 amount) {
      actualStunChance += amount;
      stunChance = actualStunChance;
      stunChance = Clamp(stunChance, Zero, maxChance);
    }
        
    public void ModifyStunChanceDuration(F32 amount) {
      StunChanceDuration += amount;
      StunChanceDuration = Clamp(StunChanceDuration, Zero, maxChance); // refactor if crit chance needs to be restored to a previous value
    }
    
    public bool CalculateStun() => random.NextF32(100) < stunChance;
    public F32 CalculateDamage() => random.NextF32(100) < critChance ? Damage * critDamageMultiplier : Damage;

    public void Reset() {
      actualStunChance = Zero;
      critChance = Zero;
      stunChance = Zero;
      StunChanceDuration = Zero;
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
    readonly F32 attackAnimationTotalTime;
    readonly F32 projectileTravelTimePerTile;
    readonly F32 critDamageMultiplier = ToF32(1.5f);
    readonly F32 maxChance = ToF32(100f);
    readonly F32 attackSpeed;
    readonly F32 sqrRange;
    F32 critChance = ToF32(15);
    F32 stunChance;
    F32 actualStunChance;
    F32 lastStartAttackTime;
  }
}