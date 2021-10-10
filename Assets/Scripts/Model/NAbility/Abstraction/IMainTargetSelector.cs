using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAbility.Abstraction {
  public interface IMainTargetSelector {
    IUnit Select(AiContext context);
  }
}