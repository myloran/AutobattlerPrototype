using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsSurrounded : BaseDecision {
    public override EDecision Type { get; } = EDecision.IsSurrounded;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsSurrounded());
    
    protected override bool GetBranch(AiContext context) => context.IsSurrounded(Unit.Coord);
  }
}