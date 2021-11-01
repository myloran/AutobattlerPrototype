using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface IAttack {
    F32 AttackAnimationHitTime { get; }
    F32 TimeToFinishAttackAnimation { get; }
    F32 AttackSpeedTime { get; }
    bool IsRanged { get; }

    void ModifyCritChance(F32 amount);
    F32 CalculateDamage();
    bool CanStartAttack(F32 currentTime);
    bool IsWithinAttackRange(IMovement target);
    F32 ProjectileTravelTimeTo(IMovement target);
    void StartAttack(F32 currentTime);
    void EndAttack();
  }
}