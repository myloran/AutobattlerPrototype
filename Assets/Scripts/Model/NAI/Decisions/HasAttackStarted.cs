using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class HasAttackStarted : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.HasAttackStarted;
    public override IDecisionTreeNode Clone() => BaseClone(this, new HasAttackStarted());
    
    protected override bool GetBranch(AiContext context) => !Unit.CanStartAttack(context.CurrentTime);
  }
}