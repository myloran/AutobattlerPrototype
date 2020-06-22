using Model.NAI.Actions;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Addons.OkwyLogging;

namespace Controller.NDebug {
  public class LoggingDecorator : IDecisionTreeNode {
    public LoggingDecorator(IDecisionTreeNode decision, DebugInfo debugInfo) {
      this.decision = decision;
      this.debugInfo = debugInfo;
    }

    public EDecision Type { get; } = EDecision.LoggingDecorator;

    public IDecisionTreeNode MakeDecision(AiContext context) {
      if (!debugInfo.IsDebugOn) return decision.MakeDecision(context);
      
      message += decision.GetType().Name + "->";
      if (decision is FindNearestTargetAction) { } else //TODO: refactor to use enum
      if (decision is BaseAction ba) {
        log.Info($"[{context.CurrentTime}] {ba.Unit.Coord} {message}");
        message = "";
      }

      return decision.MakeDecision(context);
    }

    static string message;
    readonly IDecisionTreeNode decision;
    readonly DebugInfo debugInfo;
    static readonly Logger log = MainLog.GetLogger(nameof(LoggingDecorator));
  }
}