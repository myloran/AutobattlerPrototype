using Model.NAbility;
using Model.NBattleSimulation;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Model.NUnit.Abstraction {
  public interface IAbility {
    Ability Ability { get; }
    [JsonIgnore] IUnit AbilityTarget { get; }
    public Coord AbilityTargetCoord { get; } //to test determinism
    F32 Mana { get; set; }
    F32 CastHitTime { get; }
    F32 TimeToFinishCast { get; }
    F32 TargetingSqrRange { get; }
    F32 AbilitySqrRadius { get; }
    bool HasManaAccumulated { get; }
    void AccumulateMana();
    bool IsWithinAbilityRange(AiContext context);
    bool CanStartCasting(F32 time);
    void StartCasting(F32 time);
    void EndCasting();
    void CastAbility(AiContext context);
  }
}