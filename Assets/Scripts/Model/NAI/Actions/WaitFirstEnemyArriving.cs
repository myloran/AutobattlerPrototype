using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class WaitFirstEnemyArriving : BaseAction {
    public WaitFirstEnemyArriving(Unit unit, IEventBus bus) : base(unit, bus) { }
    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var time = Unit.Target.TimeWhenDecisionWillBeExecuted - context.CurrentTime;
      var decisionCommand = new MakeDecisionCommand(Unit, context, time);
      context.InsertCommand(time, decisionCommand);
      Bus.Raise(new IdleEvent(Unit.Coord));
      return this;
    }
  }
}