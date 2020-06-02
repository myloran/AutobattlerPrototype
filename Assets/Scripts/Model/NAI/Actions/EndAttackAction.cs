using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;

namespace Model.NAI.Actions {
  public class EndAttackAction : BaseAction {
    public EndAttackAction(Unit unit, IEventBus bus) : base(unit, bus) { }
    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var health = Unit.Target.Unit.Health;
      var damage = Unit.Attack.Damage;
      var movement = Unit.Target.Unit.Movement;
      var ai = Unit.Ai;
      var attack = Unit.Attack;
      attack.EndAttack();
      var deathCommand = new DeathCommand(Unit.Movement, context); 
      var startMoveCommand = new ApplyDamageCommand(health, damage, movement, deathCommand, Bus);
      context.InsertCommand(startMoveCommand);
      var decisionCommand = new MakeDecisionCommand(ai, context, attack.AttackSpeed);
      context.InsertCommand(decisionCommand, attack.AttackSpeed);
      return this;
    }
  }
}