using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class IsAlive : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.IsAlive;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsAlive());
    
    protected override bool GetBranch(AiContext context) => Unit.IsAlive;
  }
}