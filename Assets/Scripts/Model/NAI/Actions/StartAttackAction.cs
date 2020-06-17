using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class StartAttackAction : BaseAction {
    public StartAttackAction(Unit unit, IEventBus bus) : base(unit, bus) { }

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var attack = Unit.Attack;
      var ai = Unit.Ai;
      var targetMovement = Unit.Target.Unit.Movement;
      attack.StartAttack(context.CurrentTime);
      var decisionCommand = new MakeDecisionCommand(ai, context, attack.AnimationSpeed);
      context.InsertCommand(attack.AnimationSpeed, decisionCommand);
      Bus.Raise(new RotateEvent(Unit.Movement.Coord, targetMovement.Coord));
      Bus.Raise(new StartAttackEvent(Unit.Movement.Coord));
      // var startMoveCommand = new ApplyDamageCommand(Unit.Movement, targetMovement, Bus);
      //if health == 0 execute unit death command
      return this;
    }
  }
}