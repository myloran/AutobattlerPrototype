using Model.NAbility.Abstraction;
using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Timings {
  public class PeriodTiming : ITiming {
    public F32 InitialDelay { get; }
    public F32 Period { get; }
    public bool InitialDelayHandled { get; set; }

    public PeriodTiming(F32 initialDelay, F32 period, int count) {
      Period = period;
      InitialDelay = initialDelay;
      this.count = count;
    }

    public bool HasNext() => periodsTaken < count;
    public void TakeNext() => periodsTaken++;
    
    public void Reset() {
      periodsTaken = 0;
      InitialDelayHandled = false;
    }

    readonly int count;
    int periodsTaken;
  }
}