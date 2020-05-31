using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;

namespace Model.NAI.Actions {
  public class FindNearestTargetAction : IDecisionTreeNode {
    public FindNearestTargetAction(IDecisionTreeNode decision, CTarget target, CStats stats) {
      this.decision = decision;
      this.target = target;
      this.stats = stats;
    }

    public IDecisionTreeNode MakeDecision(AiContext context) {
      target.FindNearestTarget(context.EnemyUnits(stats.Player)); //TODO: if we dont find target, we should make another decision
      return decision.MakeDecision(context);
    }

    readonly IDecisionTreeNode decision;
    readonly CTarget target;
    readonly CStats stats;
  }
}