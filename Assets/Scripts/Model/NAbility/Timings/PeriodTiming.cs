using Model.NAbility.Abstraction;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Timings {
  public class PeriodTiming : ITiming {
    public PeriodTiming(F32 period, int count) {
      this.period = period;
      this.count = count;
    }

    public bool IsTimeReset { get; set; }
    
    public bool HasNext() => ticks < count;
    public F32 GetNext() => period;
    public void Tick() => ticks++;
    
    public void Reset() {
      IsTimeReset = false;
      ticks = 0;
    }

    readonly F32 period;
    readonly int count;
    int ticks;
  }
}