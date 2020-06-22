using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Model.NAI.Decisions {
  public class CanMove : BaseDecision {
    public CanMove(IDecisionTreeNode trueNode, IDecisionTreeNode falseNode, IUnit unit) 
      : base(trueNode, falseNode, unit) { }
    
    protected override bool GetBranch(AiContext context) {
      var canMove = moveFinder.Find(Unit, context);
      Unit.NextMove = moveFinder.MoveInfo;
      return canMove;
    }
    
    static readonly MoveFinder moveFinder = new MoveFinder();
  }
}