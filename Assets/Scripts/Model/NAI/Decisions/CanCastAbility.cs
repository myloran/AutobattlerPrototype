using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class CanCastAbility : BaseDecision {
    public override EDecision Type { get; } = EDecision.CanCastAbility;
    public override IDecisionTreeNode Clone() => BaseClone(this, new CanCastAbility());

    protected override bool GetBranch(AiContext context) => Unit.HasManaAccumulated;
  }
}