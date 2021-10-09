namespace Model.NUnit.Abstraction {
  public interface IUnit : IHealth, IAttack, IMovement, ITarget, IAi, IStats, IAbility, ISilence {
    void Reset();
  }
}