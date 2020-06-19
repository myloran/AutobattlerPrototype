using FixMath;

namespace Model.NUnit.Abstraction {
  public interface IHealth {
    F32 Health { get; }
    bool IsAlive { get; }
    
    void TakeDamage(F32 damage);
    void SubToDeath(CTarget target);
    void UnsubFromDeath(CTarget target);
  }
}