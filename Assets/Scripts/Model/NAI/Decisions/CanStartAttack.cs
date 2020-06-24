using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class CanStartAttack : BaseDecision {
    public override EDecision Type { get; } = EDecision.CanStartAttack;
    public override IDecisionTreeNode Clone() => BaseClone(this, new CanStartAttack());
    
    protected override bool GetBranch(AiContext context) => 
      Unit.CanStartAttack(context.CurrentTime);
  }
}