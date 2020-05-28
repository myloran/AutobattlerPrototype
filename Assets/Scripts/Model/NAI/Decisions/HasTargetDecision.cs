using Model.NDecisionTree;
using Model.NUnit;

namespace Model.NAI.Decisions {
  public class HasTargetDecision : Decision {
    public HasTargetDecision(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, 
        CTarget target) : base(trueNode, falseNode) {
      this.target = target;
    }
    
    protected override bool GetBranch() => target.Exists;

    readonly CTarget target;
  }
}