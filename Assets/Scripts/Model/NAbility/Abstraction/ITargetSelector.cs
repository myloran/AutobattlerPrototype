using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAbility.Abstraction {
  public interface ITargetSelector {
    IUnit Select(AiContext context);
  }
}