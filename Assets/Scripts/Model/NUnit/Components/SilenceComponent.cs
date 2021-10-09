using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Const;

namespace Model.NUnit.Components {
  public class SilenceComponent : ISilence {
    public bool IsSilenced(F32 currentTime) => SilenceEndTime >= currentTime;
    public F32 SilenceEndTime { get; private set; }
    public void ApplySilence(F32 duration) => SilenceEndTime = Max(SilenceEndTime, duration);
    
    // public bool CanStartAttack(F32 currentTime) => 
    //   lastStartAttackTime + AttackAnimationHitTime < currentTime;
    
    // public void StartAttack(F32 currentTime) => lastStartAttackTime = currentTime;
    public void Reset() => SilenceEndTime = -MaxBattleDuration;

    public override string ToString() => $"{nameof(SilenceEndTime)}: {SilenceEndTime}";
  }
}