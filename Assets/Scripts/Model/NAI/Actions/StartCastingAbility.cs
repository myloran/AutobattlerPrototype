using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class StartCastingAbility : BaseAction {
    public override EDecision Type { get; } = EDecision.StartCastingAbility;
    public override IDecisionTreeNode Clone() => BaseClone(this, new StartCastingAbility());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      Unit.StartCasting(context.CurrentTime);
      
      context.InsertCommand(Unit.CastHitTime, 
        new MakeDecisionCommand(Unit, context, Unit.CastHitTime));
      
      Bus.Raise(new RotateEvent(Unit.Coord, Unit.Target.Coord));
      Bus.Raise(new StartAttackEvent(Unit.Coord));
      return this;
    }
  }
}