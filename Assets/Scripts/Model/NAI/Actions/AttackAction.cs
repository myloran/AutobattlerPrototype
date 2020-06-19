using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using static FixMath.F32;

namespace Model.NAI.Actions {
  public class AttackAction : BaseAction {
    public AttackAction(Unit unit, IEventBus bus) : base(unit, bus) { }
                    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      if (Unit.TargetExists) {
        var deathCommand = new DeathCommand(Unit.Target, context);
        var applyDamageCommand = new ApplyDamageCommand(Unit, deathCommand, Bus);
        context.InsertCommand(Zero, applyDamageCommand); //inserting to heap because units can attack at the same time
      }

      context.InsertCommand(Unit.TimeToFinishAttackAnimation, 
        new FinishAttackCommand(Unit, Bus));
      
      var time = Max(Unit.AttackSpeedTime, Unit.TimeToFinishAttackAnimation);
      context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));

      return this;
    }
  }
}