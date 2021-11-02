using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Shared.Client.Events;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Actions {
  public class RangedAttackAction : BaseAction {
    public override EDecisionTreeType Type { get; } = EDecisionTreeType.RangedAttack;
    public override IDecisionTreeNode Clone() => BaseClone(this, new RangedAttackAction());

    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var projectileTravelTime = Unit.ProjectileTravelTimeTo(Unit.Target);
      context.InsertCommand(projectileTravelTime, new ExecuteAttackCommand(Unit, context, Bus)); //inserting to heap because units can attack at the same time
      context.InsertCommand(Unit.TimeToFinishAttackAnimation, new FinishAttackCommand(Unit, Bus));
      
      var time = Max(Unit.AttackSpeedTime, Unit.TimeToFinishAttackAnimation);
      context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
      
      Bus.Raise(new SpawnProjectileEvent(Unit.Coord, Unit.Target.Coord, context.CurrentTime, projectileTravelTime));
      return this;
    }
  }
}