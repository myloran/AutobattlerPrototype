using Shared;

namespace Model.NUnit {
  public class Unit {
    public UnitInfo Info;
    public CHealth Health;
    public CAttack Attack;
    public CMovement Movement;
    public CTarget Target;
    public CAi Ai;
    public CStats Stats;

    public void Reset() {
      Health.Reset();
      Movement.Reset();
    }
  }
}