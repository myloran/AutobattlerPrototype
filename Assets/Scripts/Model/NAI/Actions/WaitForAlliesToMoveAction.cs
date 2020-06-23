using Model.NAI.Commands;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAI.Actions {
  public class WaitForAlliesToMoveAction : BaseAction {
    public override EDecision Type { get; } = EDecision.WaitForAlliesToMove;
    public WaitForAlliesToMoveAction(IUnit unit, IEventBus bus) : base(unit, bus) { }
    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var decisionCommand = new WaitForAlliesToMoveCommand(Unit, context);
      context.InsertCommand(Zero, decisionCommand);
      return this;
    }
  }
}