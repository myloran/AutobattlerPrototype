using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class ValidateTarget : Decision {
    public ValidateTarget(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode) : base(trueNode, falseNode) { }
    
    protected override bool GetBranch(AiContext context) {
      throw new System.NotImplementedException();
    }
  }
}