using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Primitives.CoordExt;

namespace Model.NAbility {
  public class AbilityComponent : IAbility {
    public Ability Ability { get; set; }
    public ISilence Silence { get; }
    public IUnit AbilityTarget { get; private set; }
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
    //refactor decision tree graph to compose multiple components into one or at least allow to accept input from multiple sources
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
      Mana = Zero;
      Ability.Reset();
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
    public bool CanStartCasting(F32 currentTime) => lastStartCastTime + CastHitTime < currentTime;
    public void StartCasting(F32 currentTime) => lastStartCastTime = currentTime;
    public void EndCasting() => lastStartCastTime = Zero;
    
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