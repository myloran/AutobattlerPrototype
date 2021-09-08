using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
  public class ComposableNode : Node {
    [Node.InputAttribute(connectionType = Node.ConnectionType.Override), HideInInspector] public bool input;

    public int Selected { get; set; }

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