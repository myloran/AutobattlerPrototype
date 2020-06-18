using FixMath;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared;
using static FixMath.F32;

namespace Model.NUnit {
  public class CAi {
    public IDecisionTreeNode CurrentDecision;
    public F32 DecisionTime;
    public F32 TimeWhenDecisionWillBeExecuted;
    public bool IsWaiting;

    public void Reset() {
      DecisionTime = Zero;
      TimeWhenDecisionWillBeExecuted = Zero;
      IsWaiting = false;
    }

    public void MakeDecision(AiContext context) {
      context.IsCyclicDecision = false;
      CurrentDecision = decisionTree.MakeDecision(context);
    }

    public void SetDecisionTree(IDecisionTreeNode decisionTree) => 
      this.decisionTree = CurrentDecision = decisionTree;

    public override string ToString() => $"{nameof(CurrentDecision)}: {CurrentDecision.GetType().Name}, {nameof(DecisionTime)}: {DecisionTime}, {nameof(TimeWhenDecisionWillBeExecuted)}: {TimeWhenDecisionWillBeExecuted}";
    
    IDecisionTreeNode decisionTree;
  }
}