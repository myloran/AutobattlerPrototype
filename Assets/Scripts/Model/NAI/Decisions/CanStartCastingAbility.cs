using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class CanStartCastingAbility : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.CanStartCastingAbility;
    public override IDecisionTreeNode Clone() => BaseClone(this, new CanStartCastingAbility());

    protected override bool GetBranch(AiContext context) => Unit.CanStartCasting(context.CurrentTime);
  }
}