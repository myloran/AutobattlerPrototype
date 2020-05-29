using Model.NBattleSimulation;

namespace Model.NDecisionTree {
  public interface IDecisionTreeNode {
    IDecisionTreeNode MakeDecision(AiContext context);
  }
}