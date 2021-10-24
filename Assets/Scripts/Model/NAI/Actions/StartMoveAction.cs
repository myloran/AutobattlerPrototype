using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class StartMoveAction : BaseAction {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.StartMove;
    public override IDecisionTreeNode Clone() => BaseClone(this, new StartMoveAction());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var (coord, time) = Unit.NextMove;
      Unit.StartMovement(context.CurrentTime + time);
      context.AddUnit(coord, Unit);
      context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
      
      Bus.Raise(new StartMoveEvent(Unit.Coord, coord, context.CurrentTime, time));
      return this;
    }
  }
}