using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class NeedTwoTileAttackRange : BaseDecision {
    public override EDecision Type { get; } = EDecision.NeedTwoTileAttackRange;
    public override IDecisionTreeNode Clone() => BaseClone(this, new NeedTwoTileAttackRange());
    
    protected override bool GetBranch(AiContext context) => 
      Unit.IsWithinAttackRange(Unit.Target);
  }
}