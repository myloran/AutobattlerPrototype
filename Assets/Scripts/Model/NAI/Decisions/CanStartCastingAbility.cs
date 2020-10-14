using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class CanStartCastingAbility : BaseDecision {
    public override EDecision Type { get; } = EDecision.CanStartCastingAbility;
    public override IDecisionTreeNode Clone() => BaseClone(this, new CanStartCastingAbility());

    protected override bool GetBranch(AiContext context) => Unit.HasManaAccumulated;
  }
}