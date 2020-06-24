using System;
using System.Collections.Generic;
using Controller.DecisionTree.Visitor;
using MessagePack;
using Model.NAI.NDecisionTree;

namespace Controller.DecisionTree.Data {
  [MessagePackObject]
  [Serializable]
  public class DecisionData : IDecisionTreeComponent {
    [Key(0)] public EDecision Type;
    [Key(1)] public List<IDecisionTreeComponent> Components = new List<IDecisionTreeComponent>();
    
    public void AddRange(IEnumerable<IDecisionTreeComponent> components) => 
      Components.AddRange(components);
    
    public T Accept<T>(IDecisionTreeDataVisitor<T> decisionTreeDataVisitor) => decisionTreeDataVisitor.VisitDecision(this);

    public override string ToString() => $"{Type}, {nameof(Components)}: {string.Join(",", Components)}";
  }
}