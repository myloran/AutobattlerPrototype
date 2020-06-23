using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class CanStartAttack : BaseDecision {
    public override EDecision Type { get; } = EDecision.CanStartAttack;

    public CanStartAttack(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, 
      IUnit unit) : base(trueNode, falseNode, unit) { }
    
    protected override bool GetBranch(AiContext context) => 
      Unit.CanStartAttack(context.CurrentTime);
  }
}