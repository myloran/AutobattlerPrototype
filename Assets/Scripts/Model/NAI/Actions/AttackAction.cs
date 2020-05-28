using Model.NAI.Visitors;
using Model.NBattleSimulation;
using Model.NDecisionTree;
using UnityEngine;

namespace Model.NAI.Actions {
  public class AttackAction : IDecisionTreeNode {
    public IDecisionTreeNode MakeDecision(AiContext context) {
      //if health == 0 execute unit death command
      Debug.Log("AttackAction");
      return this;
    }

    public void Accept(IActionVisitor visitor) => visitor.VisitAttackAction(this);
  }
}