using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.NDecisionTree {
  public interface IDecisionTreeNode {
    EDecision Type { get; }
    IUnit Unit { get; set; }
    
    IDecisionTreeNode MakeDecision(AiContext context);
    IDecisionTreeNode Clone();
  }
}