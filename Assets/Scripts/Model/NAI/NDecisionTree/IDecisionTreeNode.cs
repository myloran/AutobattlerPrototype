using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.NDecisionTree {
  public interface IDecisionTreeNode {
    EDecision Type { get; }
    IDecisionTreeNode MakeDecision(AiContext context);
    IDecisionTreeNode Clone();
    void SetUnit(IUnit unit);
  }
}