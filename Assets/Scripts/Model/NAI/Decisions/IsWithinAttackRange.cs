using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsWithinAttackRange : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.IsWithingAttackRange;
    public override IDecisionTreeNode Clone() => BaseClone(this, new IsWithinAttackRange());
    
    protected override bool GetBranch(AiContext context) => 
      Unit.IsWithinAttackRange(Unit.Target);
  }
}