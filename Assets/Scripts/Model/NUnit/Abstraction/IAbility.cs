using Model.NBattleSimulation;
using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface IAbility : ISilence {
    IUnit AbilityTarget { get; }
    F32 Mana { get; }
    F32 CastHitTime { get; }
    F32 TimeToFinishCast { get; }
    bool HasManaAccumulated { get; }
    void AccumulateMana();
    bool IsWithinAbilityRange(AiContext context);
    bool CanStartCasting(F32 currentTime);
    void StartCasting(F32 currentTime);
    void EndCasting();
    void CastAbility(AiContext context);
  }
}