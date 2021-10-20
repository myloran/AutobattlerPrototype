using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface IHealth {
    F32 Health { get; }
    bool IsAlive { get; }
    
    void TakeDamage(F32 damage);
    void SubToDeath(ITargeting targeting);
    void UnsubFromDeath(ITargeting targeting);
  }
}