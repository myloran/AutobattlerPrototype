using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Const;

namespace Model.NUnit.Components {
  public class SilenceComponent : ISilence {
    public F32 SilenceEndTime { get; private set; }
    
    public bool IsSilenced(F32 currentTime) => SilenceEndTime >= currentTime;
    public void ApplySilence(F32 endTime) => SilenceEndTime = Max(SilenceEndTime, endTime);
    public void Reset() => SilenceEndTime = -MaxBattleDuration;

    public override string ToString() => $"{nameof(SilenceEndTime)}: {SilenceEndTime}";
  }
}