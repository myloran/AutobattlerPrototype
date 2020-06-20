using Model.NAI.Actions;
using Model.NBattleSimulation;
using Shared.Addons.OkwyLogging;

namespace Model.NAI.NDecisionTree {
  public class LoggingDecorator : IDecisionTreeNode {
    public LoggingDecorator(IDecisionTreeNode decision) {
      this.decision = decision;
    }

    public EDecision Type { get; } = EDecision.LoggingDecorator;

    public IDecisionTreeNode MakeDecision(AiContext context) {
      message += decision.GetType().Name + "->";
      if (decision is FindNearestTargetAction) { } else
      if (decision is BaseAction ba) {
        log.Info($"[{context.CurrentTime}] {ba.Unit.Coord} {message}");
        message = "";
      }

      return decision.MakeDecision(context);
    }

    static string message;
    readonly IDecisionTreeNode decision;
    static readonly Logger log = MainLog.GetLogger(nameof(LoggingDecorator));
  }
}