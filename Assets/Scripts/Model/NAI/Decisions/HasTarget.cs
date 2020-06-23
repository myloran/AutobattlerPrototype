using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class HasTarget : BaseDecision {
    public override EDecision Type { get; } = EDecision.HasTarget;

    protected override bool GetBranch(AiContext context) => Unit.TargetExists;
  }
}