using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class IsWithinAbilityRange : BaseDecision {
    public override EDecision Type { get; } = EDecision.IsWithingAbilityRange;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsWithinAbilityRange());

    protected override bool GetBranch(AiContext context) => Unit.IsWithinAbilityRange(Unit.Target);
  }
}