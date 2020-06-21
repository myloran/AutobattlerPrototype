using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsPlayerDead : BaseDecision {
    public IsPlayerDead(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, IUnit unit) 
      : base(trueNode, falseNode, unit) { }

    protected override bool GetBranch(AiContext context) => context.IsPlayerDead;
  }
}