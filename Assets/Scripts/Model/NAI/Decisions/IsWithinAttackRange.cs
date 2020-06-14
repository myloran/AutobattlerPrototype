using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;

namespace Model.NAI.Decisions {
  public class IsWithinAttackRange : BaseDecision {
    public IsWithinAttackRange(IDecisionTreeNode trueNode, 
      IDecisionTreeNode falseNode, CAttack attack, CTarget target) : base(trueNode, falseNode) {
      this.attack = attack;
      this.target = target;
    }
    
    protected override bool GetBranch(AiContext context) => attack.IsWithinAttackRange(target.Unit.Movement);

    readonly CTarget target;
    readonly CAttack attack;
  }
}