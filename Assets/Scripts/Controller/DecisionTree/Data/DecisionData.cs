using System;
using Controller.DecisionTree.Nodes;
using MessagePack;
using Model.NAI.NDecisionTree;

namespace Controller.DecisionTree.Data {
  [MessagePackObject]
  [Serializable]
  public class DecisionData : DecisionTreeComponent {
    [Key(1)] public EDecision Type;
    
    public DecisionData() {}

    public DecisionData(EDecision type) => Type = type;

    public override T Accept<T>(IVisitor<T> visitor, T state) => 
      visitor.VisitDecision(this, state);

    public override string ToString() => $"{Type}, {nameof(Components)}: {string.Join(",", Components)}";
  }
}