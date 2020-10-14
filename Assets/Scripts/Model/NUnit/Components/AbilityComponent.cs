using System.Collections.Generic;
using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Primitives.CoordExt;

namespace Model.NAbility {
  public class AbilityComponent : IAbility {
    F32 mana,
      sqrRange,
      manaPerAttack,
      lastStartCastTime;

    public bool HasManaAccumulated => mana == 100;

    public void AccumulateMana() {
      mana += manaPerAttack;

      Clamp(mana, Zero, FromFloat(100));
    }

    public bool IsWithinAbilityRange(IMovement movement) {
      var targetUnit = targetSelector.Select();
      // var sqrRange = ToF32(range * range);
      return SqrDistance(movement.Coord, targetUnit.Coord) <= sqrRange;
    }
    
    public bool CanExecuteAbility(F32 currentTime) => lastStartCastTime < currentTime;
    public void StartCastingAbility(F32 currentTime) => lastStartCastTime = currentTime;
    public void EndCastingAbility() => lastStartCastTime = mana = Zero;

    // public void Execute() {
    //   if (target == ETarget.Unit && unitTargetingRule == EUnitTargetingRule.Closest) {
    //     var units = targetSelector.Select();
    //     var targetUnit = context.GetClosestUnit(unit.Coord);
    //     effect.Execute(units);
    //   }
    // }

    ITargetSelector targetSelector;
    IEffect effect;


    List<AbilityComponent> nestedAbilities = new List<AbilityComponent>();
    Unit unit;
    Coord tile;

    F32 damage;
    F32 time;
    ETarget target;
    EUnitTargetingRule unitTargetingRule;

    public AbilityComponent SelectTarget(ETarget target) {
      this.target = target;
      return this;
    }

    public AbilityComponent SelectUnitTargetingRule(EUnitTargetingRule rule) {
      unitTargetingRule = rule;
      return this;
    }
    
    public AbilityComponent SetRange(F32 range) {
      this.sqrRange = range;
      return this;
    }
    
    public AbilityComponent SetDamage(F32 damage) {
      this.damage = damage;
      return this;
    }

    // Target
    //select a unit, 
  }
}