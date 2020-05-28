using Model.NDecisionTree;
using Model.NUnit;

namespace Model.NAI.Decisions {
  public class IsWithinAttackRangeDecision : Decision {
    public IsWithinAttackRangeDecision(IDecisionTreeNode trueNode, 
      IDecisionTreeNode falseNode, CAttack attack, CTarget target) : base(trueNode, falseNode) {
      this.attack = attack;
      this.target = target;
    }
    
    protected override bool GetBranch() => attack.IsWithinAttackRange(target.Unit.Movement);

    readonly CTarget target;
    readonly CAttack attack;
  }
}