using System.Linq;
using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Actions {
  public class FindNearestTargetAction : BaseAction {
    public override EDecision Type { get; } = EDecision.FindNearestTarget;
    public override IDecisionTreeNode Clone() => BaseClone(this, new FindNearestTargetAction());
    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var units = context.EnemyUnits(Unit.Player); 
      //TODO: check if it's moving and if so record that in unit
      var target = Unit.FindNearestTarget(units);
      Unit.ChangeTargetTo(target);
      
      context.InsertCommand(Zero, new MakeDecisionCommand(Unit, context, Zero));
      return this;
    }
  }
}