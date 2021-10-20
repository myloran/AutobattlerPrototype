using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Const;
using static Shared.Primitives.CoordExt;

namespace Model.NAbility {
  public class AbilityComponent : IAbility {
    public Ability Ability { get; set; }
    [JsonIgnore] public IUnit AbilityTarget { get; private set; }
    public Coord AbilityTargetCoord => AbilityTarget?.Coord ?? Coord.Invalid; //to test determinism
    public F32 CastHitTime { get; }
    public F32 Mana { get; set; } //TODO: extract stuff related to mana into separate component
    public F32 TargetingSqrRange { get; } //TODO: move it to ability
    public F32 AbilitySqrRadius { get; }

    public AbilityComponent(IMovement movement, F32 abilitySqrRadius, F32 targetingRange, F32 manaPerAttack,
      F32 castAnimationHitTime, F32 animationTotalTime) {
      this.movement = movement;
      TargetingSqrRange = targetingRange * targetingRange;
      AbilitySqrRadius = abilitySqrRadius * abilitySqrRadius;
      this.manaPerAttack = manaPerAttack;
      CastHitTime = castAnimationHitTime;
      this.animationTotalTime = animationTotalTime;
    }
    
    public void Reset() {
      AbilityTarget = null;
      Mana = Zero;
      Ability.Reset();
      EndCasting();
    }

    public bool HasManaAccumulated => Mana == 100;

    public void AccumulateMana() {
      Mana += manaPerAttack;
      Mana = Clamp(Mana, Zero, ToF32(100));
    }

    public bool IsWithinAbilityRange(AiContext context) {
      AbilityTarget = Ability.SelectTarget(context);
      var isWithinAbilityRange = AbilityTarget != null && SqrDistance(movement.Coord, AbilityTarget.Coord) <= TargetingSqrRange;
      return isWithinAbilityRange;
    }
    
    public F32 TimeToFinishCast => animationTotalTime - CastHitTime;
    public bool CanStartCasting(F32 time) => lastStartCastTime + CastHitTime < time;
    public void StartCasting(F32 time) => lastStartCastTime = time;
    public void EndCasting() => lastStartCastTime = -MaxBattleDuration;
    
    public void CastAbility(AiContext context) {
      Reset();
      Ability.Cast(context);
    }

    public override string ToString() => $"{nameof(Mana)}: {Mana}, {nameof(lastStartCastTime)}: {lastStartCastTime}, {nameof(AbilityTarget)}: {AbilityTarget?.Coord}";
    
    readonly IMovement movement;
    readonly F32 manaPerAttack;
    readonly F32 animationTotalTime;

    F32 lastStartCastTime;
  }
}