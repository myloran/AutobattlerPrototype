using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class StartAttackAction : BaseAction {
    public StartAttackAction(IUnit unit, IEventBus bus) : base(unit, bus) { }

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      Unit.StartAttack(context.CurrentTime);
      var decisionCommand = new MakeDecisionCommand(Unit, context, Unit.AttackAnimationHitTime);
      context.InsertCommand(Unit.AttackAnimationHitTime, decisionCommand);
      Bus.Raise(new RotateEvent(Unit.Coord, Unit.Target.Coord));
      Bus.Raise(new StartAttackEvent(Unit.Coord));
      // var startMoveCommand = new ApplyDamageCommand(Unit.Movement, targetMovement, Bus);
      //if health == 0 execute unit death command
      return this;
    }
  }
}