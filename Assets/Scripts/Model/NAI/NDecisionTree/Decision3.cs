using System;
using Model.NBattleSimulation;
using Model.NUnit;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseDecision3 : IDecisionTreeNode {
    protected enum Options3 {
      First,
      Second,
      Third
    } 
    public EDecision Type { get; } = EDecision.BaseDecision;
    protected Unit Unit;
    
    protected BaseDecision3(IDecisionTreeNode firstNode, IDecisionTreeNode secondNode,
        IDecisionTreeNode thirdNode, Unit unit) {
      this.firstNode = firstNode;
      this.secondNode = secondNode;
      this.thirdNode = thirdNode;
      Unit = unit;
    }

    protected abstract Options3 GetBranch(AiContext context);

    public IDecisionTreeNode MakeDecision(AiContext context) {
      switch (GetBranch(context)) {
        case Options3.First:
          return firstNode.MakeDecision(context);
        case Options3.Second:
          return secondNode.MakeDecision(context);
        case Options3.Third:
          return thirdNode.MakeDecision(context);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    readonly IDecisionTreeNode firstNode, 
      secondNode, 
      thirdNode;
  }
}