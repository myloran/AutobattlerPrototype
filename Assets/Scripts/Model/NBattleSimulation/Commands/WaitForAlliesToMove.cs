using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NUnit;

namespace Model.NBattleSimulation.Commands {
  public class WaitForAlliesToMove : BaseCommand {
    public WaitForAlliesToMove(CMovement movement, CAi ai, AiContext context, float time = 0) {
      this.movement = movement;
      this.ai = ai;
      this.context = context;
      ai.NextDecisionTime = time;
    }

    public override void Execute() {
      var units = context.GetSurroundUnits(movement.Coord)
        .Where(u => u.Ai.CurrentDecision.Type == EDecision.MoveAction);

      if (!units.Any()) {
        log.Error("Surrounded, but allies are not moving"); //TODO: handle that case gracefully
        return;
      }
      var time = units.Min(u => u.Ai.NextDecisionTime); //check if has elements
        
      var decisionCommand = new MakeDecisionCommand(ai, context, time);
      context.InsertCommand(decisionCommand, time);
    }

    readonly CMovement movement;
    readonly CAi ai;
    readonly AiContext context;
    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(WaitForAlliesToMove));
  }
}