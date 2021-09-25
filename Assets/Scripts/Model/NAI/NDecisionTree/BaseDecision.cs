using Model.NAI.Decisions;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseDecision : IDecisionTreeNode {
    public IDecisionTreeNode TrueNode, 
      FalseNode;

    IUnit unit;
    public IUnit Unit {
      get => unit;
      set {
        unit = value;
        TrueNode.Unit = unit;
        FalseNode.Unit = unit;
      }
    }

    public virtual EDecision Type { get; } = EDecision.BaseDecision;
    
    public abstract IDecisionTreeNode Clone();

    protected IDecisionTreeNode BaseClone(BaseDecision from, BaseDecision to) {
      to.TrueNode = from.TrueNode.Clone();
      to.FalseNode = from.FalseNode.Clone();
      return to;
    }

    public void Init(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode) {
      TrueNode = trueNode;
      FalseNode = falseNode;
    }

    protected abstract bool GetBranch(AiContext context);

    public IDecisionTreeNode MakeDecision(AiContext context) {
      var branch = GetBranch(context) ? TrueNode : FalseNode;
      //TODO: add debug check if decision is the one we want to stop and exit immediately after that, so that we can see game state in our point of interest
      return branch.MakeDecision(context);
    }
  }
}