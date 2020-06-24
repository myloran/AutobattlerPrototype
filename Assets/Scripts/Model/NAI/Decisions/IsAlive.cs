using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class IsAlive : BaseDecision {
    public override EDecision Type { get; } = EDecision.IsAlive;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsAlive());
    
    protected override bool GetBranch(AiContext context) => Unit.IsAlive;
  }
}