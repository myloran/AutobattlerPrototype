using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class IsSurrounded : BaseDecision {
    public override EDecision Type { get; } = EDecision.IsSurrounded;

    public IsSurrounded(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, IUnit unit) 
      : base(trueNode, falseNode, unit) { }

    protected override bool GetBranch(AiContext context) => context.IsSurrounded(Unit.Coord);
  }
}