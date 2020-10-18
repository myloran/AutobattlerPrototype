using Model.NAI.Actions;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.OkwyLogging;

namespace Controller.NDebug {
  public class LoggingDecorator : IDecisionTreeNode {
    public EDecision Type { get; } = EDecision.LoggingDecorator;

    public LoggingDecorator(IDecisionTreeNode decision, DebugInfo debugInfo) {
      this.decision = decision;
      this.debugInfo = debugInfo;
    }

    public LoggingDecorator() { }
    
    public IDecisionTreeNode Clone() => new LoggingDecorator(decision.Clone(), debugInfo);
    
    public void SetUnit(IUnit unit) {
      this.unit = unit;
      decision.SetUnit(unit);
    }

    public IDecisionTreeNode MakeDecision(AiContext context) {
      if (!debugInfo.IsDebugOn) return decision.MakeDecision(context);
      
      message += decision.GetType().Name + "->";
      if (decision is FindNearestTargetAction) { } else //when 2 units are using that, it displays incorrectly //TODO: refactor to use enum
      if (decision is BaseAction ba) {
        log.Info($"[{context.CurrentTime}] {ba.Unit.Coord} {message}");
        message = "";
      }

      return decision.MakeDecision(context);
    }

    static string message;
    readonly IDecisionTreeNode decision;
    readonly DebugInfo debugInfo;
    IUnit unit;
    static readonly Logger log = MainLog.GetLogger(nameof(LoggingDecorator));
  }
}