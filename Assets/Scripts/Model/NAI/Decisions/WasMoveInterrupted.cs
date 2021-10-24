using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class WasMoveInterrupted : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.WasMoveInterrupted;
    public override IDecisionTreeNode Clone() => BaseClone(this, new WasMoveInterrupted());
    
    protected override bool GetBranch(AiContext context) => Unit.IsMovePaused;
  }
}