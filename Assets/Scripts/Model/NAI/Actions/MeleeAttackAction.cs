using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Actions {
  public class MeleeAttackAction : BaseAction {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.MeleeAttack;
    public override IDecisionTreeNode Clone() => BaseClone(this, new MeleeAttackAction());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      context.InsertCommand(Zero, new ApplyDamageCommand(Unit, context, Bus)); //inserting to heap because units can attack at the same time
      context.InsertCommand(Unit.TimeToFinishAttackAnimation, new FinishAttackCommand(Unit, Bus));
      
      var time = Max(Unit.AttackSpeedTime, Unit.TimeToFinishAttackAnimation);
      context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
      return this;
    }
  }
}