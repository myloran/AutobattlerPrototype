using Model.NAbility.Abstraction;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAbility {
  public class SelfTargetSelector : IMainTargetSelector {
    public SelfTargetSelector(IUnit unit) {
      this.unit = unit;
    }

    public IUnit Select(AiContext context) => unit;

    readonly IUnit unit;
  }
}