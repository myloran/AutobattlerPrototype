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
      ai.OnDecisionExecutionTimeUpdated += OnDecisionExecutionTimeUpdated;
    }

    void OnDecisionExecutionTimeUpdated(F32 time) {
      ai.OnDecisionExecutionTimeUpdated -= OnDecisionExecutionTimeUpdated;
      context.InsertCommand(time, new MakeDecisionCommand(Unit, context, time));
      isCurrentDecisionInvalidated = true;
    }

    public override void Execute() {
      if (isCurrentDecisionInvalidated) return;
      
      ai.OnDecisionExecutionTimeUpdated -= OnDecisionExecutionTimeUpdated;
      ai.MakeDecision(context);
    }

    readonly IAi ai;
    readonly AiContext context;
    bool isCurrentDecisionInvalidated;
  }
}