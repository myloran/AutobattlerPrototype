using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsAlive : BaseDecision {
    public override EDecision Type { get; } = EDecision.IsAlive;

    public IsAlive(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, IUnit unit) 
      : base(trueNode, falseNode, unit) { }
    
    protected override bool GetBranch(AiContext context) => Unit.IsAlive;
  }
}