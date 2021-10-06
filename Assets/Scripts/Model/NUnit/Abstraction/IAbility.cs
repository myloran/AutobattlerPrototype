using Model.NBattleSimulation;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Model.NUnit.Abstraction {
  public interface IAbility : ISilence {
    [JsonIgnore] IUnit AbilityTarget { get; }
    public Coord AbilityTargetCoord { get; } //to test determinism
    F32 Mana { get; }
    F32 CastHitTime { get; }
    F32 TimeToFinishCast { get; }
    bool HasManaAccumulated { get; }
    void AccumulateMana();
    bool IsWithinAbilityRange(AiContext context);
    bool CanStartCasting(F32 time);
    void StartCasting(F32 time);
    void EndCasting();
    void CastAbility(AiContext context);
  }
}