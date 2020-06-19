using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using static Shared.Const;

namespace Model.NAI.Decisions {
  public class CheckEnemiesArrivingToAdjacentTile : BaseDecision3 {
    public CheckEnemiesArrivingToAdjacentTile(IDecisionTreeNode firstNode, 
        IDecisionTreeNode secondNode, IDecisionTreeNode thirdNode, CMovement movement, 
        CStats stats, Unit unit) : base(firstNode, secondNode, thirdNode) {
      this.movement = movement;
      this.stats = stats;
      this.unit = unit;
    }
    
    protected override Options3 GetBranch(AiContext context) {
      var units = context.GetAdjacentUnits(movement.Coord)
        .Where(u => !u.IsAllyWith(stats.Player));
      if (!units.Any()) return Options3.First;

      var unit = units.MinBy(u => u.Ai.TimeWhenDecisionWillBeExecuted);
      var timeToArrive = unit.Ai.TimeWhenDecisionWillBeExecuted - context.CurrentTime;
      this.unit.ChangeTargetTo(unit);

      return timeToArrive < StraightMoveTime ? Options3.Second : Options3.Third;
    }

    readonly CMovement movement;
    readonly CStats stats;
    readonly Unit unit;
  }
}