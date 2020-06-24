using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;

namespace Model.NAI.Actions {
  public class StartAttackAction : BaseAction {
    public override EDecision Type { get; } = EDecision.StartAttack;
    public override IDecisionTreeNode Clone() => BaseClone(this, new StartAttackAction());
    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      Unit.StartAttack(context.CurrentTime);
      
      context.InsertCommand(Unit.AttackAnimationHitTime, 
        new MakeDecisionCommand(Unit, context, Unit.AttackAnimationHitTime));
      
      Bus.Raise(new RotateEvent(Unit.Coord, Unit.Target.Coord));
      Bus.Raise(new StartAttackEvent(Unit.Coord));
      return this;
    }
  }
}