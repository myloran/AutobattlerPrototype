using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsAlive : BaseDecision {
    public override EDecision Type { get; } = EDecision.IsAlive;

    protected override bool GetBranch(AiContext context) => Unit.IsAlive;
  }
}