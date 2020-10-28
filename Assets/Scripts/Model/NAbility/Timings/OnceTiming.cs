using Model.NAbility.Abstraction;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Timings {
  public class OnceTiming : ITiming {
    public bool HasNext() => !isTriggered;

    public F32 GetNext() {
      isTriggered = true;
      return F32.Zero;
    }

    bool isTriggered;
  }
}