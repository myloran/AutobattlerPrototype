using System;
using Controller.DecisionTree.Nodes;
using Controller.DecisionTree.Visitor;
using MessagePack;
using Model.NAI.NDecisionTree;

namespace Controller.DecisionTree.Data {
  [MessagePackObject]
  [Serializable]
  public class ActionData : IDecisionTreeComponent {
    [Key(0)] public EDecision Type;
    
    public T Accept<T>(IDecisionTreeDataVisitor<T> decisionTreeDataVisitor) => decisionTreeDataVisitor.VisitAction(this);
    
    public override string ToString() => $"{Type}";
  }
}