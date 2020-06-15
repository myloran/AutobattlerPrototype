using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using static Shared.Const;

namespace Model.NAI.Actions {
  public class WaitMoveDiff : BaseAction {
    public WaitMoveDiff(Unit unit, IEventBus bus) : base(unit, bus) { }
    
    public override IDecisionTreeNode MakeDecision(AiContext context) {
      var decisionCommand = new MakeDecisionCommand(Unit.Ai, context, MoveDiffTime);
      context.InsertCommand(MoveDiffTime, decisionCommand);
      return this;
    }
  }
}