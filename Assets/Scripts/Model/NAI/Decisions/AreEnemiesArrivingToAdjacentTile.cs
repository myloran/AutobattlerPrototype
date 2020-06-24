using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Decisions {
  //TODO: Refactor to BaseDecision?
  public class AreEnemiesArrivingToAdjacentTile : BaseDecision {
    public override EDecision Type { get; } = EDecision.AreEnemiesArrivingToAdjacentTile;
    public override IDecisionTreeNode Clone() => BaseClone(this, new AreEnemiesArrivingToAdjacentTile());

    protected override bool GetBranch(AiContext context) {
      Unit.ArrivingTargets = context.GetAdjacentUnits(Unit.Coord)
        .Where(u => !u.IsAllyWith(Unit.Player));
      return Unit.ArrivingTargets.Any();
    }
  }
}