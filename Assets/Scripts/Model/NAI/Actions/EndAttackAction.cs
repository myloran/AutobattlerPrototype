using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class EndAttackAction : BaseAction {
    public EndAttackAction(Unit unit, IEventBus bus) : base(unit, bus) { }
                    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var attack = Unit.Attack;
      attack.EndAttack();
      
      var target = Unit.Target;
      if (target.Exists) {
        var targetHealth = target.Unit.Health;
        var targetMovement = target.Unit.Movement;
        var damage = Unit.Attack.Damage;
        var deathCommand = new DeathCommand(targetMovement, context);
        var applyDamageCommand = new ApplyDamageCommand(targetHealth, damage, 
          targetMovement, deathCommand, Bus);
        Bus.Raise(new IdleEvent(Unit.Movement.Coord));
        context.InsertCommand(0, applyDamageCommand);  
      }
      
      var decisionCommand = new MakeDecisionCommand(Unit.Ai, context, attack.AttackSpeed);
      context.InsertCommand(attack.AttackSpeed, decisionCommand);
      return this;
    }
  }
}