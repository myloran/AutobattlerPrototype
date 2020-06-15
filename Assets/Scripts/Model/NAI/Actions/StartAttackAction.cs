using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;

namespace Model.NAI.Actions {
  public class StartAttackAction : BaseAction {
    public StartAttackAction(Unit unit, IEventBus bus) : base(unit, bus) { }

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var attack = Unit.Attack;
      var ai = Unit.Ai;
      attack.StartAttack(context.CurrentTime);
      var decisionCommand = new MakeDecisionCommand(ai, context, attack.AnimationSpeed);
      context.InsertCommand(attack.AnimationSpeed, decisionCommand);
      //if health == 0 execute unit death command
      return this;
    }
  }
}