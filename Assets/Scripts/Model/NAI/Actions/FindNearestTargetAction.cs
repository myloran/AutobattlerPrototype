using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using PlasticFloor.EventBus;

namespace Model.NAI.Actions {
  public class FindNearestTargetAction : BaseAction {
    public FindNearestTargetAction(Unit unit, IEventBus bus, IDecisionTreeNode decision) : base(unit, bus) {
      this.decision = decision;
    }

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var target = Unit.Target;
      var stats = Unit.Stats;
      target.FindNearestTarget(context.EnemyUnits(stats.Player)); //TODO: if we dont find target, we should make another decision
      
      return decision.MakeDecision(context);
    }

    readonly IDecisionTreeNode decision;
  }
}