using Model.NBattleSimulation;
using Model.NDecisionTree;

namespace Model.NAI.Actions {
  public class AttackAction : IDecisionTreeNode {
    public IDecisionTreeNode MakeDecision(AiContext context) {
      //if health == 0 execute unit death command
      return this;
    }
  }
}