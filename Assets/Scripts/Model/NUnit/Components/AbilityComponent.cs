using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Primitives.CoordExt;

namespace Model.NAbility {
  public class AbilityComponent : IAbility {
    public Ability Ability { get; set; }
    public IUnit AbilityTarget { get; private set; }
    public F32 CastHitTime { get; }
    public F32 Mana { get; private set; }

    public AbilityComponent(IMovement movement, F32 sqrRange, F32 manaPerAttack, F32 castAnimationHitTime, 
        F32 animationTotalTime) {
      this.sqrRange = sqrRange;
      this.manaPerAttack = manaPerAttack;
      CastHitTime = castAnimationHitTime;
      this.animationTotalTime = animationTotalTime;
      this.movement = movement;
    }
    
    public void Reset() => Mana = Zero;

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
      Ability.Cast(context);
      Mana = Zero;
    }

    public override string ToString() => $"{nameof(Mana)}: {Mana}, {nameof(lastStartCastTime)}: {lastStartCastTime}";
    
    readonly IMovement movement;
    
    readonly F32 sqrRange, //TODO: move it to ability
      manaPerAttack,
      animationTotalTime;

    F32 lastStartCastTime;
  }
}