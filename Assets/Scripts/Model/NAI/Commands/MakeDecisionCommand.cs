using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;

namespace Model.NAI.Commands {
  public class MakeDecisionCommand : BaseCommand {
    public override ECommand Type { get; } = ECommand.MakeDecision;

    public MakeDecisionCommand(IAi ai, AiContext context, F32 time) {
      this.ai = ai;
      this.context = context;
      ai.SetDecisionTime(context.CurrentTime, time);
    }

    public override void Execute() => ai.MakeDecision(context); //TODO: remove reference on context from ai

    readonly IAi ai;
    readonly AiContext context;
  }
}