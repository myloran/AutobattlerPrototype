using System;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;

namespace Model.NAI.Commands {
  public class MakeDecisionCommand : BaseCommand {
    public override ECommand Type { get; } = ECommand.MakeDecision;

    public MakeDecisionCommand(IUnit ai, AiContext context, F32 time) : base(ai) {
      this.ai = ai;
      this.context = context;
      ai.SetDecisionTime(context.CurrentTime, time);
      ai.OnDecisionExecutionTimeUpdated += OnDecisionExecutionTimeUpdated(context);
    }

    Action<F32> OnDecisionExecutionTimeUpdated(AiContext context) =>
      newTime => {
        ai.OnDecisionExecutionTimeUpdated -= OnDecisionExecutionTimeUpdated(context);
        context.InsertCommand(newTime, new MakeDecisionCommand(Unit, context, newTime));
        isCurrentDecisionInvalidated = true;
      };

    public override void Execute() {
      if (isCurrentDecisionInvalidated) return;
      
      ai.OnDecisionExecutionTimeUpdated -= OnDecisionExecutionTimeUpdated(context);
      ai.MakeDecision(context);
    }

    readonly IAi ai;
    readonly AiContext context;
    bool isCurrentDecisionInvalidated;
  }
}