using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Abstraction {
  public interface ITiming {
    F32 Period { get; }
    
    bool HasNext();
    F32 GetNext(F32 currentTime);
    void TakeNext(F32 currentTime);
    void Reset();
  }
}