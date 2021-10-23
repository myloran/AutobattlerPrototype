using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface IStun {
    F32 StunEndTime { get; }
    void ApplyStun(F32 endTime);
  }
}