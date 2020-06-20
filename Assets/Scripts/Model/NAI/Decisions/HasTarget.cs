using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class HasTarget : BaseDecision {
    public HasTarget(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, 
      IUnit unit) : base(trueNode, falseNode, unit) { }
    
    protected override bool GetBranch(AiContext context) => Unit.TargetExists;
  }
}