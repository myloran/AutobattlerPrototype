using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;

namespace Model.NAI.Decisions {
  public class CanStartAttack : BaseDecision {
    public CanStartAttack(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, 
        CAttack attack) : base(trueNode, falseNode) {
      this.attack = attack;
    }
    
    protected override bool GetBranch(AiContext context) => 
      attack.CanStartAttack(context.CurrentTime);

    readonly CAttack attack;
  }
}