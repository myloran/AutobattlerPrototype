using UnityEngine;
using XNode;

namespace Data.DecisionTree.Nodes {
  public class DecisionTreeSaverNode : Node {
    [Output, HideInInspector] public bool output;
    
    public override object GetValue(NodePort port) {
      return null; // Replace this
    }
  }
}