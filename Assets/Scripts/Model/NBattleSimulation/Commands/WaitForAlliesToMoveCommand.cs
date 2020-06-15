using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NUnit;

namespace Model.NBattleSimulation.Commands {
  public class WaitForAlliesToMoveCommand : BaseCommand {
    public WaitForAlliesToMoveCommand(CMovement movement, CAi ai, AiContext context) {
      this.movement = movement;
      this.ai = ai;
      this.context = context;
    }

    public override void Execute() { //TODO; wait MoveDiffTime instead of putting decision into queue end 
      var units = context.GetSurroundUnits(movement.Coord)
        .Where(u => u.Ai.CurrentDecision.Type == EDecision.MoveAction);

      if (!units.Any()) {
        log.Error("Surrounded, but allies are not moving"); //TODO: handle that case gracefully
        return;
      }
      var time = units.Min(u => u.Ai.DecisionTime);
        
      var decisionCommand = new MakeDecisionCommand(ai, context, time); //TODO: replace with ai.MakeDecision?
      context.InsertCommand(time, decisionCommand);
    }

    readonly CMovement movement;
    readonly CAi ai;
    readonly AiContext context;
    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(WaitForAlliesToMoveCommand));
  }
}