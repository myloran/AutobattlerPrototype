using Model.NBattleSimulation;

namespace Model.NAI.NDecisionTree {
  public interface IDecisionTreeNode {
    IDecisionTreeNode MakeDecision(AiContext context);
  }
}