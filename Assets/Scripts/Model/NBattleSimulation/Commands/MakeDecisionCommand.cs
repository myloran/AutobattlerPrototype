using FixMath;
using Model.NUnit.Abstraction;

namespace Model.NBattleSimulation.Commands {
  public class MakeDecisionCommand : BaseCommand {
    public MakeDecisionCommand(IAi ai, AiContext context, F32 time) {
      this.ai = ai;
      this.context = context;
      ai.DecisionTime = time;
      ai.TimeWhenDecisionWillBeExecuted = context.CurrentTime + time;
    }

    public override void Execute() => ai.MakeDecision(context);

    readonly IAi ai;
    readonly AiContext context;
  }
}