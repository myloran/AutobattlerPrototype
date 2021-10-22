using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  public class AreEnemiesArrivingToAdjacentTile : BaseDecision {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.AreEnemiesArrivingToAdjacentTile;
    public override IDecisionTreeNode Clone() => BaseClone(this, new AreEnemiesArrivingToAdjacentTile());

    protected override bool GetBranch(AiContext context) {
      Unit.ArrivingTargets = context.GetAdjacentUnits(Unit.Coord).Where(u => !u.IsAllyWith(Unit.Player));
      
      if (Unit.IsTaunted(context.CurrentTime)) 
        Unit.ArrivingTargets = Unit.ArrivingTargets.Where(u => u == Unit.Target);
      
      return Unit.ArrivingTargets.Any();
    }
  }
}