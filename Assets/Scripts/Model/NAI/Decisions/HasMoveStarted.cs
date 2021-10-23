using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class HasMoveStarted : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.HasMoveStarted;
    public override IDecisionTreeNode Clone() => BaseClone(this, new HasMoveStarted());
    
    protected override bool GetBranch(AiContext context) => !Unit.CanStartMove(context.CurrentTime);
  }
}