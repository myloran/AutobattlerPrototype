using System;
using Controller.DecisionTree.Visitor;
using MessagePack;

namespace Controller.DecisionTree.Data {
  [MessagePackObject]
  [Serializable]
  public class DecisionData : DecisionTreeComponent {
    [Key(1)] public DecisionTreeComponent OnTrue;
    [Key(2)] public DecisionTreeComponent OnFalse;
    
    public override T Accept<T>(IDecisionTreeDataVisitor<T> decisionTreeDataVisitor) => 
      decisionTreeDataVisitor.VisitDecision(this);

    public override string ToString() => $"{Type}, {OnTrue} {OnFalse}";
  }
}