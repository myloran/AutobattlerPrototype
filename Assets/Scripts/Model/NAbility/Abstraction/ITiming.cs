using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Abstraction {
  public interface ITiming {
    F32 InitialDelay { get; }
    F32 Period { get; }
    bool InitialDelayHandled { get; set; }

    bool HasNext();
    void TakeNext();
    void Reset();
  }
}