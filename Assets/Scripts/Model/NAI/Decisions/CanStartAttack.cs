using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;

namespace Model.NAI.Decisions {
  public class CanStartAttack : BaseDecision {
    public CanStartAttack(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, 
        Unit unit) : base(trueNode, falseNode, unit) { }
    
    protected override bool GetBranch(AiContext context) => 
      Unit.CanStartAttack(context.CurrentTime);
  }
}