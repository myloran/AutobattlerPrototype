using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseDecision : IDecisionTreeNode {
    public EDecision Type { get; } = EDecision.BaseDecision;
    protected readonly IUnit Unit;
    
    protected BaseDecision(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, IUnit unit) {
      this.trueNode = trueNode;
      this.falseNode = falseNode;
      Unit = unit;
    }

    protected abstract bool GetBranch(AiContext context);

    public IDecisionTreeNode MakeDecision(AiContext context) {
      var branch = GetBranch(context) ? trueNode : falseNode;
      return branch.MakeDecision(context);
    }

    readonly IDecisionTreeNode trueNode, falseNode;
  }
}