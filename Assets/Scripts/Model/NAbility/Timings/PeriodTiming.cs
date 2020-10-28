using Model.NAbility.Abstraction;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Timings {
  public class PeriodTiming : ITiming {
    public PeriodTiming(F32 period, int count, F32 initialDelay) {
      this.period = period;
      this.count = count;
      this.initialDelay = initialDelay;
    }

    public bool HasNext() => current < count;
    public F32 GetNext() => initialDelay + ++current*period;

    readonly F32 period;
    readonly int count;
    readonly F32 initialDelay;
    int current;
  }
}