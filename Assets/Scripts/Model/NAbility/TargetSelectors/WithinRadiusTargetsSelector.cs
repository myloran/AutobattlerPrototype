using System;
using System.Collections.Generic;
using System.Linq;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NAbility {
  public class WithinRadiusTargetsSelector : IAdditionalTargetsSelector {
    public WithinRadiusTargetsSelector(IUnit unit, Func<Coord> getAbilityOrigin) {
      this.unit = unit;
      this.getAbilityOrigin = getAbilityOrigin;
    }

    public IEnumerable<IUnit> Select(IUnit target, AiContext context) {
      var abilityOrigin = getAbilityOrigin();
      return target != null && abilityOrigin != Coord.Invalid
        ? context.FindUnitsWithinSqrRange(abilityOrigin, target.Player, unit.AbilitySqrRadius)
        : Enumerable.Empty<IUnit>();
    }

    readonly IUnit unit;
    readonly Func<Coord> getAbilityOrigin;
  }
}