using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Shared;

namespace Model.NUnit {
  public class CAi {
    public IDecisionTreeNode Decision;
    public TimePoint NextDecisionTime;

    public IDecisionTreeNode MakeDecision(AiContext context) => Decision.MakeDecision(context);
  }
}