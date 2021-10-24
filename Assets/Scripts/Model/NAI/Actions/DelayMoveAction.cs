using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class DelayMoveAction : BaseAction {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.DelayMove;
    public override IDecisionTreeNode Clone() => BaseClone(this, new DelayMoveAction());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      Unit.StartMovement(context.CurrentTime + Unit.MovementTimeLeft);
      context.AddUnit(Unit.TakenCoord, Unit);
      context.InsertCommand(Unit.MovementTimeLeft, new MakeDecisionCommand(Unit, context, Unit.MovementTimeLeft));
      
      //ContinueMoveEvent
      // Bus.Raise(new StartMoveEvent(Unit.Coord, Unit.TakenCoord, context.CurrentTime, time));
      return this;
    }
  }
}