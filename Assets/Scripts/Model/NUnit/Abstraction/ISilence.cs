using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface ISilence {
    bool IsSilenced(F32 currentTime);
    F32 SilenceEndTime { get; }
    void ApplySilence(F32 duration);
  }
}