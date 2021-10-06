using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NAbility {
  public class ClosestUnitTargetSelector : ITargetSelector {
    public ClosestUnitTargetSelector(AbilityInfo info, IUnit unit) {
      this.info = info;
      this.unit = unit;
    }

    public IUnit Select(AiContext context) {
      if (info.UnitTargetingRule == EUnitTargetingRule.Closest) {
        return context.FindClosestUnitTo(unit.Coord, unit.Player.Opposite());
      }

      //TODO: handle null target properly, maybe check if i ability target exist first
      return null;
    }

    readonly AbilityInfo info;
    readonly IUnit unit;
  }
}