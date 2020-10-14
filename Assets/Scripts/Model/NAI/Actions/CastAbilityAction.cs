using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Actions {
  public class CastAbilityAction : BaseAction {
    public override EDecision Type { get; } = EDecision.CastAbility;
    public override IDecisionTreeNode Clone() => BaseClone(this, new CastAbilityAction());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      // Unit.ExecuteAbility();
      // context.InsertCommand(Zero, new ApplyDamageCommand(Unit, context, Bus)); //inserting to heap because units can attack at the same time
      // context.InsertCommand(Unit.TimeToFinishAttackAnimation, new FinishAttackCommand(Unit, Bus));
      //
      // var time = Max(Unit.AttackSpeedTime, Unit.TimeToFinishAttackAnimation);
      // context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
      return this;
    }
  }
}