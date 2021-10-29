using Model.NAbility.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAbility.Timings {
  public class OnceTiming : ITiming {
    public F32 Period { get; }
    public bool HasNext() => hasNext;
    public F32 GetNext(F32 currentTime) => Zero;
    public void TakeNext(F32 currentTime) => hasNext = false;
    public void Reset() => hasNext = true;

    bool hasNext;
  }
}