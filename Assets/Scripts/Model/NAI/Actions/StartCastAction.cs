using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class StartCastAction : BaseAction {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.StartCastAction;
    public override IDecisionTreeNode Clone() => BaseClone(this, new StartCastAction());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      Unit.StartCasting(context.CurrentTime);
      context.InsertCommand(Unit.CastHitTime, new MakeDecisionCommand(Unit, context, Unit.CastHitTime));
      Bus.Raise(new RotateEvent(Unit.Coord, Unit.AbilityTarget.Coord));
      Bus.Raise(new StartCastEvent(Unit.Coord));
      return this;
    }
  }
}