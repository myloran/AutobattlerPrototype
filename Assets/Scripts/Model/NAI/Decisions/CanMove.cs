using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class CanMove : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.CanMove;
    public override IDecisionTreeNode Clone() => BaseClone(this, new CanMove());

    protected override bool GetBranch(AiContext context) {
      var canMove = moveFinder.Find(Unit, context);
      Unit.NextMove = moveFinder.MoveInfo;
      return canMove;
    }
    
    static readonly MoveFinder moveFinder = new MoveFinder();
  }
}