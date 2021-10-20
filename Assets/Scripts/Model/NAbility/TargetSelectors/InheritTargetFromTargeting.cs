using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;

namespace Model.NAbility {
  public class InheritTargetFromTargeting : IMainTargetSelector {
    public InheritTargetFromTargeting(IUnit unit) {
      this.unit = unit;
    }

    public IUnit Select(AiContext context) => unit.Target;

    readonly IUnit unit;
  }
}