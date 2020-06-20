using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using static Shared.Const;

namespace Model.NAI.Decisions {
  public class CheckEnemiesArrivingToAdjacentTile : BaseDecision3 {
    public CheckEnemiesArrivingToAdjacentTile(IDecisionTreeNode firstNode, 
        IDecisionTreeNode secondNode, IDecisionTreeNode thirdNode, 
        IUnit unit) : base(firstNode, secondNode, thirdNode, unit) { }
    
    protected override Options3 GetBranch(AiContext context) {
      var targets = context.GetAdjacentUnits(Unit.Coord)
        .Where(u => !u.IsAllyWith(Unit.Player));
      if (!targets.Any()) return Options3.First;

      var target = targets.MinBy(u => u.TimeWhenDecisionWillBeExecuted);
      var timeToArrive = target.TimeWhenDecisionWillBeExecuted - context.CurrentTime;
      Unit.ChangeTargetTo(target);

      return timeToArrive < StraightMoveTime ? Options3.Second : Options3.Third;
    }
  }
}