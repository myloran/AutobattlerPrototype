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
    public ISilence Silence { get; }
    [JsonIgnore] public IUnit AbilityTarget { get; private set; }
    public Coord AbilityTargetCoord => AbilityTarget?.Coord ?? Coord.Invalid; //to test determinism
    public F32 CastHitTime { get; }
    public F32 Mana { get; private set; } //TODO: extract stuff related to mana into separate component

    public AbilityComponent(IMovement movement, F32 sqrRange, F32 manaPerAttack, F32 castAnimationHitTime, 
        F32 animationTotalTime, ISilence silence) {
      this.sqrRange = sqrRange;
      this.manaPerAttack = manaPerAttack;
      CastHitTime = castAnimationHitTime;
      this.animationTotalTime = animationTotalTime;
      this.movement = movement;
      Silence = silence;
    }
    //TODO: THE PLAN, don't forget about it
    //create IsSilencedDecision
    //subscribe to silence event
    //react to silence event(clean up ability casting logic, notify view, make new instant decision)
    //display silence on the view properly

    #region Silence

    public bool IsSilenced => Silence.IsSilenced;
    public F32 SilenceDuration => Silence.SilenceDuration;
    public void ApplySilence(F32 duration) => Silence.ApplySilence(duration);
    
    #endregion
    
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
      return SqrDistance(movement.Coord, AbilityTarget.Coord) <= sqrRange;
    }
    
    public F32 TimeToFinishCast => animationTotalTime - CastHitTime;
    public bool CanStartCasting(F32 time) => lastStartCastTime + CastHitTime < time;
    public void StartCasting(F32 time) => lastStartCastTime = time;
    public void EndCasting() => lastStartCastTime = -MaxBattleDuration;
    
    public void CastAbility(AiContext context) {
      Reset();
      Ability.Cast(context);
    }

    public override string ToString() => $"{nameof(Mana)}: {Mana}, {nameof(lastStartCastTime)}: {lastStartCastTime}";
    
    readonly IMovement movement;
    
    readonly F32 sqrRange, //TODO: move it to ability
      manaPerAttack,
      animationTotalTime;

    F32 lastStartCastTime;
  }
}