using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;

namespace Model.NAI.Commands {
  public class MakeDecisionCommand : BaseCommand {
    public MakeDecisionCommand(IAi ai, AiContext context, F32 time) {
      this.ai = ai;
      this.context = context;
      //TODO: make method
      ai.DecisionTime = time;
      ai.TimeWhenDecisionWillBeExecuted = context.CurrentTime + time;
    }

    public override void Execute() => ai.MakeDecision(context);

    readonly IAi ai;
    readonly AiContext context;
  }
}