using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class HasTarget : BaseDecision {
    public override EDecision Type { get; } = EDecision.HasTarget;
    public override IDecisionTreeNode Clone() => BaseClone(this, new HasTarget());
    
    protected override bool GetBranch(AiContext context) => Unit.TargetExists;
  }
}