using Model.NDecisionTree;

namespace Model.NAI.Decisions {
  public class ValidateTargetDecision : Decision {
    public ValidateTargetDecision(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode) : base(trueNode, falseNode) { }
    
    protected override bool GetBranch() {
      throw new System.NotImplementedException();
    }
  }
}