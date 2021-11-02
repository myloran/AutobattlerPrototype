using System;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NUnit.Components {
  public class AiComponent : IAi {
    public IDecisionTreeNode CurrentDecision { get; private set; }
    public F32 DecisionTime { get; private set; }
    public F32 TimeWhenDecisionWillBeExecuted { get; private set; }
    public Action<F32> OnDecisionExecutionTimeUpdated { get; set; } = time => { };

    public void Reset() {
      CurrentDecision = null;
      DecisionTime = Zero;
      TimeWhenDecisionWillBeExecuted = Zero;
      OnDecisionExecutionTimeUpdated = _ => { };
    }

    public void SetDecisionTime(F32 currentTime, F32 time) {
      DecisionTime = time;
      TimeWhenDecisionWillBeExecuted = currentTime + time;
    }

    public void MakeDecision(AiContext context) => 
      CurrentDecision = decisionTree.MakeDecision(context);

    public void SetDecisionTree(IDecisionTreeNode decisionTree) => 
      this.decisionTree = decisionTree;
    
    public void UpdateNextDecisionExecutionTime(F32 time) => OnDecisionExecutionTimeUpdated(time);

    public override string ToString() => $"{nameof(CurrentDecision)}: {CurrentDecision?.GetType().Name}, {nameof(DecisionTime)}: {DecisionTime}, {nameof(TimeWhenDecisionWillBeExecuted)}: {TimeWhenDecisionWillBeExecuted}";
    
    IDecisionTreeNode decisionTree;
  }
}