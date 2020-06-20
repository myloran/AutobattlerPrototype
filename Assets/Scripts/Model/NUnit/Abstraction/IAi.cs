using FixMath;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NUnit.Abstraction {
  public interface IAi {
    IDecisionTreeNode CurrentDecision { get; }
    F32 DecisionTime { get; set; }
    F32 TimeWhenDecisionWillBeExecuted { get; set; }
    bool IsWaiting { get; set; }
    void MakeDecision(AiContext context);
    void SetDecisionTree(IDecisionTreeNode decisionTree);
  }
}