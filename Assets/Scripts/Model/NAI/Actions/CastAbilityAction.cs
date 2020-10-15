using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Actions {
  public class CastAbilityAction : BaseAction {
    public override EDecision Type { get; } = EDecision.CastAbility;
    public override IDecisionTreeNode Clone() => BaseClone(this, new CastAbilityAction());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      context.InsertCommand(Zero, new CastAbilityCommand(Unit, context)); //inserting to heap because units can attack at the same time
      context.InsertCommand(Unit.TimeToFinishCast, new FinishCastCommand(Unit, Bus));
      
      var time = Max(Unit.AttackSpeedTime, Unit.TimeToFinishCast); //think if attack speed needs to be replaced with ability speed
      context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
      return this;
    }
  }
}