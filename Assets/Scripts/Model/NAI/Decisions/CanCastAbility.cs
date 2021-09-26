using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class CanCastAbility : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.CanCastAbility;
    public override IDecisionTreeNode Clone() => BaseClone(this, new CanCastAbility());

    protected override bool GetBranch(AiContext context) => Unit.HasManaAccumulated;
  }
}