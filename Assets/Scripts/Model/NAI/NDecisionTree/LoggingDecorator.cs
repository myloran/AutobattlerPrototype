using Model.NAI.Actions;
using Model.NBattleSimulation;

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
        log.Info($"[{context.CurrentTime}] {ba.Unit.Movement.Coord} {message}");
        message = "";
      }

      return decision.MakeDecision(context);
    }

    static string message;
    readonly IDecisionTreeNode decision;
    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(LoggingDecorator));
  }
}