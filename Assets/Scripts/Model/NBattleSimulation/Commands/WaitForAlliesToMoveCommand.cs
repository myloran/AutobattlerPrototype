using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NUnit;
using Shared.OkwyLogging;

namespace Model.NBattleSimulation.Commands {
  public class WaitForAlliesToMoveCommand : BaseCommand {
    public WaitForAlliesToMoveCommand(Unit unit, CAi ai, AiContext context) {
      this.unit = unit;
      this.ai = ai;
      this.context = context;
    }

    public override void Execute() { //TODO; wait MoveDiffTime instead of putting decision into queue end 
      var units = context.GetSurroundUnits(unit.Coord)
        .Where(u => u.Ai.CurrentDecision.Type == EDecision.MoveAction);

      if (!units.Any()) {
        log.Error("Surrounded, but allies are not moving"); //TODO: handle that case gracefully
        return;
      }
      var time = units.Min(u => u.Ai.DecisionTime);
        
      var decisionCommand = new MakeDecisionCommand(ai, context, time); //TODO: replace with ai.MakeDecision?
      context.InsertCommand(time, decisionCommand);
    }

    readonly Unit unit;
    readonly CAi ai;
    readonly AiContext context;
    static readonly Logger log = MainLog.GetLogger(nameof(WaitForAlliesToMoveCommand));
  }
}