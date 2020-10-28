using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface ISilence {
    bool IsSilenced { get; }
    F32 SilenceDuration { get; }
    void ApplySilence(F32 duration);
  }
}