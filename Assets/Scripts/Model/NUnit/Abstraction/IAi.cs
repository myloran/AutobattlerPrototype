using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared.Addons.Examples.FixMath;

namespace Model.NUnit.Abstraction {
  public interface IAi {
    IDecisionTreeNode CurrentDecision { get; }
    F32 DecisionTime { get; }
    F32 TimeWhenDecisionWillBeExecuted { get; }
    void MakeDecision(AiContext context);
    void SetDecisionTime(F32 currentTime, F32 time);
  }
}