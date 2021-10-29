using Model.NAbility.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Const;

namespace Model.NAbility.Timings {
  public class PeriodTiming : ITiming {
    public F32 Period { get; }

    public PeriodTiming(F32 period, int count, F32 initialDelay) {
      Period = period;
      this.count = count;
      this.initialDelay = initialDelay;
    }

    public bool HasNext() => periodsTaken < count;
    
    public F32 GetNext(F32 currentTime) {
      if (lastPeriodTime == -MaxBattleDuration)
        return initialDelay;
      return currentTime - lastPeriodTime;
    }

    public void TakeNext(F32 currentTime) {
      lastPeriodTime = currentTime + Period; 
      periodsTaken++;
    }

    public void Reset() {
      lastPeriodTime = -MaxBattleDuration;
      periodsTaken = 0;
    }

    readonly F32 initialDelay;
    readonly int count;
    F32 lastPeriodTime;
    int periodsTaken;
  }
}