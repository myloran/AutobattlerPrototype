using Model.NAbility.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAbility.Timings {
  public class OnceTiming : ITiming {
    public bool IsTimeReset { get; set; }
    
    public bool HasNext() => !isTicked;
    public F32 GetNext() => Zero;
    public void Tick() => isTicked = true;
    
    public void Reset() {
      IsTimeReset = false;
      isTicked = false;
    }

    bool isTicked;
  }
}