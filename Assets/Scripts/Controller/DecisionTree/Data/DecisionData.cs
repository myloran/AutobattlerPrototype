using System;
using Controller.DecisionTree.Visitor;
using MessagePack;
using Model.NAI.NDecisionTree;

namespace Controller.DecisionTree.Data {
  [MessagePackObject]
  [Serializable]
  public class DecisionData : DecisionTreeComponent {
    [Key(1)] public EDecision Type;
    
    public DecisionData() {}

    public DecisionData(EDecision type) => Type = type;

    public override T Accept<T>(IVisitor<T> visitor) => visitor.VisitDecision(this);

    public override string ToString() => $"{Type}, {nameof(Components)}: {string.Join(",", Components)}";
  }
}