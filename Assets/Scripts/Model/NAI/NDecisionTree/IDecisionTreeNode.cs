using Model.NAI.Commands;
using Model.NBattleSimulation;

namespace Model.NAI.NDecisionTree {
  public interface IDecisionTreeNode {
    EDecision Type { get; }
    IDecisionTreeNode MakeDecision(AiContext context);
  }
}