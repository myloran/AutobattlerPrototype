using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class IsSilenced : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.IsSilenced;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsSilenced());
    
    protected override bool GetBranch(AiContext context) => Unit.IsSilenced;
  }
}