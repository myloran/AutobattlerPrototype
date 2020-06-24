using System;
using System.Collections.Generic;
using Controller.DecisionTree.Visitor;
using MessagePack;
using Model.NAI.NDecisionTree;

namespace Controller.DecisionTree.Data {
  [MessagePackObject]
  [Serializable]
  public class DecisionData : DecisionTreeComponent {
    [Key(1)] public List<DecisionTreeComponent> Components = new List<DecisionTreeComponent>();
    
    public void AddRange(IEnumerable<DecisionTreeComponent> components) => 
      Components.AddRange(components);
    
    public override T Accept<T>(IDecisionTreeDataVisitor<T> decisionTreeDataVisitor) => 
      decisionTreeDataVisitor.VisitDecision(this);

    public override string ToString() => $"{Type}, {nameof(Components)}: {string.Join(",", Components)}";
  }
}