using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.NDecisionTree {
  public interface IDecisionTreeNode {
    EDecisionTreeType Type { get; }
    IUnit Unit { get; set; }
    
    IDecisionTreeNode MakeDecision(AiContext context);
    IDecisionTreeNode Clone();
  }
}