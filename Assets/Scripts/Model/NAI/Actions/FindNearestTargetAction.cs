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
      target.Unit = context.FindNearestTarget(Unit.Stats.Player, Unit.Movement.Coord); //TODO: if we dont find target, we should make another decision
      target.Unit.Health.SubToDeath(Unit.Target);
      return decision.MakeDecision(context);
    }

    readonly IDecisionTreeNode decision;
  }
}