using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface IAbility {
    bool HasManaAccumulated { get; }
    void AccumulateMana();
    bool IsWithinAbilityRange(IMovement movement);
    void StartCastingAbility(F32 currentTime);
    void EndCastingAbility();
  }
}