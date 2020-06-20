using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared.OkwyLogging;

namespace Model.NBattleSimulation.Commands {
  public class WaitForAlliesToMoveCommand : BaseCommand {
    public WaitForAlliesToMoveCommand(IUnit unit, AiContext context) {
      this.unit = unit;
      this.context = context;
    }

    public override void Execute() {
      var units = context.GetSurroundUnits(unit.Coord)
        .Where(u => u.CurrentDecision.Type == EDecision.MoveAction);

      if (!units.Any()) {
        log.Error("Surrounded, but allies are not moving"); //TODO: handle that case gracefully
        return;
      }
      var time = units.Min(u => u.DecisionTime);
        
      var decisionCommand = new MakeDecisionCommand(unit, context, time); //TODO: replace with ai.MakeDecision?
      context.InsertCommand(time, decisionCommand);
    }

    readonly IUnit unit;
    readonly AiContext context;
    static readonly Logger log = MainLog.GetLogger(nameof(WaitForAlliesToMoveCommand));
  }
}