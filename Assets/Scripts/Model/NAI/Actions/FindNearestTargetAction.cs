using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Actions {
  public class FindNearestTargetAction : BaseAction {
    public override EDecision Type { get; } = EDecision.FindNearestTarget;
    public override IDecisionTreeNode Clone() => BaseClone(this, new FindNearestTargetAction());
    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      //TODO: check if it's moving and if so record that in unit
      var target = context.FindClosestUnitTo(Unit.Coord, Unit.Player.Opposite());
      Unit.ChangeTargetTo(target);
      
      context.InsertCommand(Zero, new MakeDecisionCommand(Unit, context, Zero));
      return this;
    }
  }
}