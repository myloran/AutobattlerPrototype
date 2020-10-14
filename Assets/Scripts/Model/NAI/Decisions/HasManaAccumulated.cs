using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class HasManaAccumulated : BaseDecision {
    public override EDecision Type { get; } = EDecision.HasManaAccumulated;
    public override IDecisionTreeNode Clone() => BaseClone(this, new HasManaAccumulated());

    protected override bool GetBranch(AiContext context) => Unit.HasManaAccumulated;
  }
}