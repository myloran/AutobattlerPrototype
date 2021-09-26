using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsSurrounded : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.IsSurrounded;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsSurrounded());
    
    protected override bool GetBranch(AiContext context) => context.IsSurrounded(Unit.Coord);
  }
}