using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NAI.Actions {
  public class FindNearestTargetAction : BaseAction {
    public FindNearestTargetAction(IUnit unit, IEventBus bus, IDecisionTreeNode decision) : base(unit, bus) {
      this.decision = decision;
    }

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var units = context.EnemyUnits(Unit.Player);
      var (hasTarget, target) = Unit.FindNearestTarget(units); //TODO: if we dont find target, we should make another decision
      if (!hasTarget) return this;
      
      Unit.ChangeTargetTo(target);
      return decision.MakeDecision(context);
    }

    readonly IDecisionTreeNode decision;
  }
}