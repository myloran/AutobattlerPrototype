using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;
using static Shared.Const;

namespace Model.NAI.Actions {
  public class WaitDiffBetweenDiagonalAndStraightMove : BaseAction {
    public override EDecision Type { get; } = EDecision.WaitDiffBetweenDiagonalAndStraightMove;

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var decisionCommand = new MakeDecisionCommand(Unit, context, MoveDiffTime);
      context.InsertCommand(MoveDiffTime, decisionCommand);
      
      Bus.Raise(new IdleEvent(Unit.Coord));
      return this;
    }
  }
}