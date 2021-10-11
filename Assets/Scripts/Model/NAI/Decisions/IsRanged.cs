using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class IsRanged : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.IsRanged;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsRanged());
    
    protected override bool GetBranch(AiContext context) => Unit.IsRanged;
  }
}