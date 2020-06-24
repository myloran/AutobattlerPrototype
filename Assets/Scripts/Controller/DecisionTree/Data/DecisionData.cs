using System;
using System.Collections.Generic;
using Controller.DecisionTree.Visitor;
using MessagePack;
using Model.NAI.NDecisionTree;

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