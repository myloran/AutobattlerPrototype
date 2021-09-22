using System.Linq;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.OkwyLogging;

namespace Model.NAI.Commands {
  public class WaitForAlliesToMoveCommand : BaseCommand {
    public WaitForAlliesToMoveCommand(IUnit unit, AiContext context) : base(unit) {
      this.unit = unit;
      this.context = context;
    }

    public override void Execute() {
      var units = context.GetSurroundUnits(unit.Coord)
        .Where(u => u.CurrentDecision.Type == EDecision.Move);

      if (!units.Any()) {
        log.Error("Surrounded, but allies are not moving"); //TODO: handle that case
        return;
      }
      var time = units.Min(u => u.DecisionTime);
      context.InsertCommand(time, new MakeDecisionCommand(unit, context, time));
    }

    readonly IUnit unit;
    readonly AiContext context;
    static readonly Logger log = MainLog.GetLogger(nameof(WaitForAlliesToMoveCommand));
  }
}