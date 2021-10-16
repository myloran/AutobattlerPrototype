using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NAbility {
  public class AlongLineTargetsSelector : IAdditionalTargetsSelector {
    public AlongLineTargetsSelector(IMovement unit) {
      this.unit = unit;
    }

    public IEnumerable<IUnit> Select(IUnit target, AiContext context) {
      if (target != null) {
        foreach (Coord coord in Bresenham.New(target.Coord, unit.Coord)) {
          var additionalTarget = context.TryGetUnit(coord);
          if (additionalTarget != null && additionalTarget.Player == target.Player) yield return additionalTarget;
        }
      }
    }

    readonly IMovement unit;
  }
}