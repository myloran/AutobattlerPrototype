using Model.NAbility.Abstraction;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Timings {
  public class OnceTiming : ITiming {
    public F32 InitialDelay { get; }
    public F32 Period { get; }
    public bool InitialDelayHandled { get; set; }
    public bool HasNext() => hasNext;
    public void TakeNext() => hasNext = false;
    public void Reset() => hasNext = true;

    bool hasNext;
  }
}