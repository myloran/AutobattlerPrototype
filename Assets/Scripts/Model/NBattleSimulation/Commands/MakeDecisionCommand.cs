using FixMath;
using Model.NAI.NDecisionTree;
using Model.NUnit;

namespace Model.NBattleSimulation.Commands {
  public class MakeDecisionCommand : BaseCommand {
    public MakeDecisionCommand(CAi ai, AiContext context, F32 time) {
      this.ai = ai;
      this.context = context;
      ai.DecisionTime = time;
      ai.TimeWhenDecisionWillBeExecuted = context.CurrentTime + time;
    }

    public override void Execute() => ai.MakeDecision(context);

    readonly CAi ai;
    readonly AiContext context;
  }
}