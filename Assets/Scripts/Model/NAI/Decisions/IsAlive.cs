using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;

namespace Model.NAI.Decisions {
  public class IsAlive : BaseDecision {
    public IsAlive(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, CHealth health) : base(trueNode, falseNode) {
      this.health = health;
    }
    
    protected override bool GetBranch(AiContext context) => health.IsAlive;

    readonly CHealth health;
  }
}