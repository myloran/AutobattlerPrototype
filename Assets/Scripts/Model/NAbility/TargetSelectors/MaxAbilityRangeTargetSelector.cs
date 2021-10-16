using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NAbility {
  public class MaxAbilityRangeTargetSelector : IMainTargetSelector {
    public MaxAbilityRangeTargetSelector(AbilityInfo info, IUnit unit) {
      this.info = info;
      this.unit = unit;
    }

    public IUnit Select(AiContext context) => 
      context.TryFindUnitOnMaxAbilityRange(unit.Coord, unit.AbilitySqrRange, unit.Player.Opposite());

    readonly AbilityInfo info;
    readonly IUnit unit;
  }
}