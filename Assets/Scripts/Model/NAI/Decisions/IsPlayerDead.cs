using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsPlayerDead : BaseDecision {
    public override EDecision Type { get; } = EDecision.IsPlayerDead;

    protected override bool GetBranch(AiContext context) => context.IsPlayerDead;
  }
}