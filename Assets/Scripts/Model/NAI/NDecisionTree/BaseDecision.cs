using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseDecision : IDecisionTreeNode {
    public IDecisionTreeNode TrueNode, 
      FalseNode;
    public IUnit Unit;

    public virtual EDecision Type { get; } = EDecision.BaseDecision;

    public void Init(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, IUnit unit) {
      this.TrueNode = trueNode;
      this.FalseNode = falseNode;
      Unit = unit;
    }

    protected abstract bool GetBranch(AiContext context);

    public IDecisionTreeNode MakeDecision(AiContext context) {
      var branch = GetBranch(context) ? TrueNode : FalseNode;
      return branch.MakeDecision(context);
    }
  }
}