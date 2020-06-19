using System.Text;
using FixMath;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Abstraction;

namespace Model.NUnit {
  public class Unit : IUnit, IHealth, IAttack, ITarget {
    readonly CHealth health;
    readonly CAttack attack;
    readonly CTarget target;
    public CMovement Movement;
    public CAi Ai;
    public CStats Stats;

    public Unit(CHealth health, CAttack attack, CMovement movement, CTarget target, CAi ai, CStats stats, EPlayer player) {
      this.health = health;
      this.attack = attack;
      Movement = movement;
      this.target = target;
      Ai = ai;
      Stats = stats;
      Player = player;
    }

    public EPlayer Player { get; set; }
    public bool IsAllyWith(EPlayer player) => Player == player;

    public void Reset() {
      health.Reset();
      attack.Reset();
      Movement.Reset();
      target.Reset();
      Ai.Reset();
      Stats.Reset();
    }

    public override string ToString() => new StringBuilder()
      .Append(health).Append("\n")
      .Append(attack).Append("\n")
      .Append(Movement).Append("\n")
      .Append(Ai).Append("\n")
      .Append(Stats).Append("\n")
      .Append(target).Append("\n")
      .ToString();

    public F32 Health => health.Health;
    public bool IsAlive => health.IsAlive;
    public void TakeDamage(F32 damage) => health.TakeDamage(damage);
    public void SubToDeath(CTarget target) => health.SubToDeath(target);
    public void UnsubFromDeath(CTarget target) => health.UnsubFromDeath(target);
    
    public F32 Damage => attack.Damage;
    public F32 AttackAnimationHitTime => attack.AttackAnimationHitTime;
    public F32 TimeToFinishAttackAnimation => attack.TimeToFinishAttackAnimation;
    public F32 AttackSpeedTime => attack.AttackSpeedTime;
    public bool CanStartAttack(F32 currentTime) => attack.CanStartAttack(currentTime);
    public bool IsWithinAttackRange(CMovement target) => attack.IsWithinAttackRange(target);
    public void StartAttack(F32 currentTime) => attack.StartAttack(currentTime);
    public void EndAttack() => attack.EndAttack();
    
    public Unit Target => target.Target;
    public bool TargetExists => target.TargetExists;
    public void ClearTarget() => target.ClearTarget();
    public void ChangeTargetTo(Unit unit) => target.ChangeTargetTo(unit);
  }
}