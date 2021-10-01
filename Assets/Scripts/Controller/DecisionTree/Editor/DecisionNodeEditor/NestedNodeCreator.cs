using Controller.DecisionTree.Nodes;
using UnityEditor;
using XNode;
using XNodeEditor;

namespace Controller.DecisionTree.Editor.DecisionNodeEditor {
  public class NestedNodeCreator {
    public NestedNodeCreator(DecisionNode target) {
      this.target = target;
    }

    public void CreateNestedDecisionTreeNode(DecisionTreeGraph nestedGraph) {
      var currentGraphEditor = NodeEditorWindow.current.graphEditor;
      var node = (NestedDecisionTreeNode) currentGraphEditor.CreateNode(typeof(NestedDecisionTreeNode), target.position);
      node.Graph = nestedGraph;
      var port = node.GetPort(nameof(node.Input));
      var otherPort = DisconnectCurrentNode();
      port.Connect(otherPort);
    }		
		
    NodePort DisconnectCurrentNode() {
      var currentNode = target as DecisionNode;
      var currentPort = currentNode.GetPort(nameof(currentNode.Input));
      var otherPort = currentPort.Connection;
      Undo.RecordObject(currentNode, "Disconnect Port");
      Undo.RecordObject(otherPort.node, "Disconnect Port");
      currentPort.Disconnect(otherPort);
      return otherPort;
    }

    readonly DecisionNode target;
  }
}