using Model.NAI.Visitors;
using Model.NBattleSimulation;

namespace Model.NDecisionTree {
  public abstract class Decision : IDecisionTreeNode {
    protected Decision(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode) {
      this.trueNode = trueNode;
      this.falseNode = falseNode;
    }

    protected abstract bool GetBranch();

    public IDecisionTreeNode MakeDecision(AiContext context) {
      var branch = GetBranch() ? trueNode : falseNode;
      return branch.MakeDecision(context);
    }

    public void Accept(IActionVisitor visitor) { }

    readonly IDecisionTreeNode trueNode, falseNode;
  }
}