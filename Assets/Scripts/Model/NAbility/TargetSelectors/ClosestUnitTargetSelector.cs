using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NAbility {
  public class ClosestUnitTargetSelector : IMainTargetSelector {
    public ClosestUnitTargetSelector(IUnit unit) {
      this.unit = unit;
    }

    public IUnit Select(AiContext context) => context.FindClosestUnitTo(unit.Coord, unit.Player.Opposite());

    readonly IUnit unit;
  }
}