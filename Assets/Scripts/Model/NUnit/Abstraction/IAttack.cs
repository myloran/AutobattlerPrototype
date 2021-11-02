using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface IAttack {
    F32 AttackAnimationHitTime { get; }
    F32 TimeToFinishAttackAnimation { get; }
    F32 AttackSpeedTime { get; }
    Property StunChanceDuration { get; }
    Property SilenceChanceDuration { get; }
    bool IsRanged { get; }

    bool CalculateStun();
    void ModifyStunChance(F32 amount);
    void ModifyStunChanceDuration(F32 amount);

    bool CalculateSilence();
    void ModifySilenceChance(F32 amount);
    void ModifySilenceChanceDuration(F32 amount);
    
    F32 CalculateDamage();
    void ModifyCritChance(F32 amount);
    
    F32 ProjectileTravelTimeTo(IMovement target);
    bool CanStartAttack(F32 currentTime);
    bool IsWithinAttackRange(IMovement target);
    void StartAttack(F32 currentTime);
    void EndAttack();
  }
}