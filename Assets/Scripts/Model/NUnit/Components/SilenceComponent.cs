using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Components {
  public class SilenceComponent : ISilence {
    public bool IsSilenced => SilenceDuration > 0;
    public F32 SilenceDuration { get; private set; }
    public void ApplySilence(F32 duration) => SilenceDuration = F32.Max(SilenceDuration, duration);
  }
}