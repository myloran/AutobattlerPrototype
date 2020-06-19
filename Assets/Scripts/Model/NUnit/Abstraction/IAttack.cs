using FixMath;

namespace Model.NUnit.Abstraction {
  public interface IAttack {
    F32 Damage { get; }
    F32 AttackAnimationHitTime { get; }
    F32 TimeToFinishAttackAnimation { get; }
    F32 AttackSpeedTime { get; }
    bool CanStartAttack(F32 currentTime);
    bool IsWithinAttackRange(IMovement target);
    void StartAttack(F32 currentTime);
    void EndAttack();
  }
}