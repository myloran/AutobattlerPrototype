using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;

namespace Model.NAI.Actions {
  public class StartCastingAbility : BaseAction {
    public override EDecision Type { get; } = EDecision.StartCastingAbility;
    public override IDecisionTreeNode Clone() => BaseClone(this, new StartCastingAbility());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      // Unit.ExecuteAbility();
      Unit.StartCastingAbility(context.CurrentTime);
      
      // context.InsertCommand(Unit.AttackAnimationHitTime, 
      //   new MakeDecisionCommand(Unit, context, Unit.AttackAnimationHitTime));
      //
      // Bus.Raise(new RotateEvent(Unit.Coord, Unit.Target.Coord));
      // Bus.Raise(new StartAttackEvent(Unit.Coord));
      return this;
    }
  }
}