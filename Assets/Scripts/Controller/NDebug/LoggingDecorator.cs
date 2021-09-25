using System;
using Model.NAI.Actions;
using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Addons.OkwyLogging;

namespace Controller.NDebug {
  public class LoggingDecorator : IDecisionTreeNode {
    public EDecision Type { get; } = EDecision.LoggingDecorator;

    IUnit unit;
    public IUnit Unit {
      get => unit;
      set {
        unit = value; 
        decision.Unit = unit;
      }
    }

    public LoggingDecorator(IDecisionTreeNode decision, DebugInfo debugInfo, Action makeDecisionLog) {
      this.decision = decision;
      this.debugInfo = debugInfo;
      this.makeDecisionLog = makeDecisionLog;
    }

    //test if removing that does not break xnode serialization or whatever
    public LoggingDecorator() { }
    
    public IDecisionTreeNode Clone() => new LoggingDecorator(decision.Clone(), debugInfo, makeDecisionLog);

    public IDecisionTreeNode MakeDecision(AiContext context) {
      if (!debugInfo.IsDebugOn) return decision.MakeDecision(context);
      
      message += decision.GetType().Name + "->";
      if (decision is FindNearestTargetAction) { } else //when 2 units are using that, it displays incorrectly //TODO: refactor to use enum
      if (decision is BaseAction ba) {
        log.Info($"[{context.CurrentTime}] {ba.Unit.Coord} {message}");
        makeDecisionLog();
        message = "";
      }

      try {
        return decision.MakeDecision(context);
      }
      catch (Exception) {
        if (message != "") {
          log.Info($"[{context.CurrentTime}] {decision.Unit.Coord} {message}");
          message = "";
        }
        throw;
      }
    }

    static string message;
    readonly IDecisionTreeNode decision;
    readonly DebugInfo debugInfo;
    readonly Action makeDecisionLog;
    static readonly Logger log = MainLog.GetLogger(nameof(LoggingDecorator));
  }
}