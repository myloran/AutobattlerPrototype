using Model.NAI.Visitors;
using Model.NBattleSimulation;

namespace Model.NDecisionTree {
  public interface IDecisionTreeNode {
    IDecisionTreeNode MakeDecision(AiContext context);
    void Accept(IActionVisitor visitor);
  }
}