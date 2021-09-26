using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NAI.Actions {
  public class MoveAction : BaseAction {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.Move;
    public override IDecisionTreeNode Clone() => BaseClone(this, new MoveAction());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var (coord, time) = Unit.NextMove;
      new StartMoveCommand(context, Unit, coord, time, Bus).Execute(); 
      
      context.InsertCommand(time, new FinishMoveCommand(context, Unit, coord, Bus));
      context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
      return this;
    }
  }
}