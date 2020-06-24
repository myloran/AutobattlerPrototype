using System;
using Controller.DecisionTree.Nodes;
using Controller.DecisionTree.Visitor;
using MessagePack;
using Model.NAI.NDecisionTree;

namespace Controller.DecisionTree.Data {
  [MessagePackObject]
  [Serializable]
  public class ActionData : DecisionTreeComponent {
    [Key(1)] public EDecision Type;
    
    public ActionData() {}

    public ActionData(EDecision action) => Type = action;

    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitAction(this);
    
    public override string ToString() => $"{Type}";
  }
}