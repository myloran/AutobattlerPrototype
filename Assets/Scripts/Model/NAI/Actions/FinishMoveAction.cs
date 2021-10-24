using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Shared.Client.Events;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Actions {
  public class FinishMoveAction : BaseAction {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.FinishMove;
    public override IDecisionTreeNode Clone() => BaseClone(this, new FinishMoveAction());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var from = Unit.Coord;
      var to = Unit.NextMove.Coord;
      
      Unit.FinishMovement();
      if (!Unit.IsTaunted(context.CurrentTime)) Unit.ClearTarget();
      
      context.RemoveUnit(from);
      context.InsertCommand(Zero, new MakeDecisionCommand(Unit, context, Zero));
      
      Bus.Raise(new FinishMoveEvent(from, to));
      return this;
    }
  }
}