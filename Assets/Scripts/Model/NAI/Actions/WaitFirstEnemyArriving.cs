using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class WaitFirstEnemyArriving : BaseAction {
    public WaitFirstEnemyArriving(IUnit unit, IEventBus bus) : base(unit, bus) { }
    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var time = Unit.Target.TimeWhenDecisionWillBeExecuted - context.CurrentTime;
      context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
      
      Bus.Raise(new IdleEvent(Unit.Coord));
      return this;
    }
  }
}