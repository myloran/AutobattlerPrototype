namespace Model.NUnit.Abstraction {
  public interface IUnit : IHealth, IAttack, IMovement, ITargeting, IAi, IStats, IAbility, ISilence, IStun {
    void Reset();
  }
}