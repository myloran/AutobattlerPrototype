using System;
using System.Collections.Generic;
using Controller.DecisionTree.Nodes;
using Controller.DecisionTree.Visitor;
using MessagePack;

namespace Controller.DecisionTree.Data {
  [Union(0, typeof(ActionData))]
  [Union(1, typeof(DecisionData))]
  [MessagePackObject]
  [Serializable]
  public abstract class DecisionTreeComponent {
    [Key(0)] public List<DecisionTreeComponent> Components = new List<DecisionTreeComponent>();
    
    public void AddRange(IEnumerable<DecisionTreeComponent> components) => 
      Components.AddRange(components);
    
    public abstract T Accept<T>(IVisitor<T> visitor);

    public override string ToString() => $"{nameof(Components)}: {string.Join(",", Components)}";
  }
}