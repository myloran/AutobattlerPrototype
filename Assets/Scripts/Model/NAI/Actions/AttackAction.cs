using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Actions {
  public class AttackAction : BaseAction {
    public AttackAction(IUnit unit, IEventBus bus) : base(unit, bus) { }
                    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      context.InsertCommand(Zero, new ApplyDamageCommand(Unit, context, Bus)); //inserting to heap because units can attack at the same time
      context.InsertCommand(Unit.TimeToFinishAttackAnimation, new FinishAttackCommand(Unit, Bus));
      
      var time = Max(Unit.AttackSpeedTime, Unit.TimeToFinishAttackAnimation);
      context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
      return this;
    }
  }
}