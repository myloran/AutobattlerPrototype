using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsPlayerDead : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.IsPlayerDead;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsPlayerDead());
    
    protected override bool GetBranch(AiContext context) => context.IsPlayerDead;
  }
}