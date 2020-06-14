using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared;

namespace Model.NUnit {
  public class CAi {
    public IDecisionTreeNode Decision;
    public TimePoint NextDecisionTime;

    public IDecisionTreeNode MakeDecision(AiContext context) {
      context.IsCyclicDecision = false;
      return Decision.MakeDecision(context);
    }

    public override string ToString() {
      return $"{nameof(Decision)}: {Decision}, {nameof(NextDecisionTime)}: {NextDecisionTime}";
    }
  }
}