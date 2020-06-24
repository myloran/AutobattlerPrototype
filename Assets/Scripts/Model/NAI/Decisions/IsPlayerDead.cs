using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsPlayerDead : BaseDecision {
    public override EDecision Type { get; } = EDecision.IsPlayerDead;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsPlayerDead());
    
    protected override bool GetBranch(AiContext context) => context.IsPlayerDead;
  }
}