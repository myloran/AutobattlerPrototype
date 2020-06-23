using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsWithinAttackRange : BaseDecision {
    public override EDecision Type { get; } = EDecision.IsWithingAttackRange;
    
    protected override bool GetBranch(AiContext context) => 
      Unit.IsWithinAttackRange(Unit.Target);
  }
}