using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Abstraction {
  public interface ITiming {
    bool IsTimeReset { get; set; }
    
    bool HasNext();
    F32 GetNext();
    void Tick();
    void Reset();
  }
}