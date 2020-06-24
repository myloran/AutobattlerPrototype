using System;
using Controller.DecisionTree.Visitor;
using MessagePack;

namespace Controller.DecisionTree.Data {
  [MessagePackObject]
  [Serializable]
  public class ActionData : DecisionTreeComponent {
    public override T Accept<T>(IDecisionTreeDataVisitor<T> decisionTreeDataVisitor) => 
      decisionTreeDataVisitor.VisitAction(this);
    
    public override string ToString() => $"{Type}";
  }
}