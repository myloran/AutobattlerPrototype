using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;

namespace Model.NAI.Decisions {
  public class HasTarget : Decision {
    public HasTarget(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, 
        CTarget target) : base(trueNode, falseNode) {
      this.target = target;
    }
    
    protected override bool GetBranch(AiContext context) => target.Exists;

    readonly CTarget target;
  }
}