using System;
using FixMath;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;
using static FixMath.F32;

namespace Model.NAI.Actions {
  public class AttackAction : BaseAction {
    public AttackAction(Unit unit, IEventBus bus) : base(unit, bus) { }
                    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var attack = Unit;
      
      var target = Unit;
      if (target.TargetExists) {
        var targetMovement = target.Target.Movement;
        var damage = Unit.Damage;
        var deathCommand = new DeathCommand(targetMovement, context);
        var applyDamageCommand = new ApplyDamageCommand(target.Target, damage, 
          targetMovement, deathCommand, Bus);
        context.InsertCommand(Zero, applyDamageCommand); //inserting to heap because units can attack at the same time
      }

      context.InsertCommand(attack.TimeToFinishAttackAnimation, 
        new FinishAttackCommand(Unit, Unit.Movement, Bus));
      
      var time = Max(attack.AttackSpeedTime, attack.TimeToFinishAttackAnimation);
      context.InsertCommand(time, new MakeDecisionCommand(Unit.Ai, context, time));

      return this;
    }
  }
}