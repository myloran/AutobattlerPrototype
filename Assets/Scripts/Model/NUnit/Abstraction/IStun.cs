using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface IStun {
    void ApplyStun(F32 currentTime, F32 duration);
  }
}