using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NUnit.Components {
  public class CAi : IAi {
    public IDecisionTreeNode CurrentDecision { get; private set; }
    public F32 DecisionTime { get; set; }
    public F32 TimeWhenDecisionWillBeExecuted { get; set; }
    public bool IsWaiting { get; set; }

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