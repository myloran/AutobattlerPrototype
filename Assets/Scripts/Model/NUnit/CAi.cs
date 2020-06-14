using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared;

namespace Model.NUnit {
  public class CAi {
    public IDecisionTreeNode DecisionTree,
      CurrentDecision,
      TemporalDecision;
    public TimePoint NextDecisionTime;
    public bool IsWaiting;

    public void MakeDecision(AiContext context) {
      context.IsCyclicDecision = false;
      CurrentDecision = DecisionTree.MakeDecision(context);
    }

    public void MakeTemporalDecision(AiContext context) {
      context.IsCyclicDecision = false;
      CurrentDecision = TemporalDecision.MakeDecision(context);
    }
    
    public void SetDecisionTree(IDecisionTreeNode decisionTree) => 
      DecisionTree = CurrentDecision = decisionTree;

    public override string ToString() => $"{nameof(DecisionTree)}: {DecisionTree}, {nameof(NextDecisionTime)}: {NextDecisionTime}";
  }
}