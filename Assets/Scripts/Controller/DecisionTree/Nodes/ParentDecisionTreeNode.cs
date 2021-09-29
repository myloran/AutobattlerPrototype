using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
  public class ParentDecisionTreeNode : Node {
    [Output(connectionType = ConnectionType.Override), HideInInspector] public bool Output;

    public DecisionTreeGraph Graph;
    
    // Use this for initialization
    protected override void Init() {
      base.Init();
		
    }

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
      return null; // Replace this
    }
  }
}