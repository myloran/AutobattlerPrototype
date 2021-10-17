using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NAbility {
  public class MaxAbilityRangeTargetSelector : IMainTargetSelector {
    public MaxAbilityRangeTargetSelector(IUnit unit) {
      this.unit = unit;
    }

    public IUnit Select(AiContext context) => 
      context.TryFindUnitOnMaxAbilityRange(unit.Coord, unit.TargetingSqrRange, unit.Player.Opposite());

    readonly IUnit unit;
  }
}