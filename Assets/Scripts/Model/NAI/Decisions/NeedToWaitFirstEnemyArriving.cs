using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Exts;
using static Shared.Const;

namespace Model.NAI.Decisions {
  public class NeedToWaitFirstEnemyArriving : BaseDecision {
    public override EDecision Type { get; } = EDecision.NeedToWaitFirstEnemyArriving;

    protected override bool GetBranch(AiContext context) {
      var target = Unit.ArrivingTargets.MinBy(u => u.TimeWhenDecisionWillBeExecuted);
      var timeToArrive = target.TimeWhenDecisionWillBeExecuted - context.CurrentTime;
      Unit.ChangeTargetTo(target);

      return timeToArrive < StraightMoveTime ? true : false;
    }
  }
}