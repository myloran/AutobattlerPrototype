using Model.NBattleSimulation;

namespace Model.NAI.NDecisionTree {
  public abstract class Decision : IDecisionTreeNode {
    protected Decision(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode) {
      this.trueNode = trueNode;
      this.falseNode = falseNode;
    }

    protected abstract bool GetBranch(AiContext context);

    public IDecisionTreeNode MakeDecision(AiContext context) {
      var branch = GetBranch(context) ? trueNode : falseNode;
      return branch.MakeDecision(context);
    }

    readonly IDecisionTreeNode trueNode, falseNode;
  }
}