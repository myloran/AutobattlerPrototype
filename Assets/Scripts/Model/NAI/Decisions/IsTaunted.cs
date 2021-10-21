using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class IsTaunted : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.IsTaunted;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsTaunted());
    
    protected override bool GetBranch(AiContext context) => Unit.IsTaunted;
  }
}