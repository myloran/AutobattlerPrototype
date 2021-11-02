using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface IAttack {
    F32 AttackAnimationHitTime { get; }
    F32 TimeToFinishAttackAnimation { get; }
    F32 AttackSpeedTime { get; }
    F32 StunChanceDuration { get; }
    bool IsRanged { get; }

    void ModifyCritChance(F32 amount);
    bool CalculateStun();
    void ModifyStunChance(F32 amount);
    void ModifyStunChanceDuration(F32 amount);
    F32 CalculateDamage();
    bool CanStartAttack(F32 currentTime);
    bool IsWithinAttackRange(IMovement target);
    F32 ProjectileTravelTimeTo(IMovement target);
    void StartAttack(F32 currentTime);
    void EndAttack();
  }
}