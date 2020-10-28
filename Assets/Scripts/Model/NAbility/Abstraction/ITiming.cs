using Shared.Addons.Examples.FixMath;

namespace Model.NAbility.Abstraction {
  public interface ITiming {
    bool HasNext();
    F32 GetNext();
  }
}