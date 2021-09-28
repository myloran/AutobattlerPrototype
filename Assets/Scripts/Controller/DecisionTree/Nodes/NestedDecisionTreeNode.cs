using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
  public class NestedDecisionTreeNode : Node {
    [Input(connectionType = ConnectionType.Override), HideInInspector] public bool Input;

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