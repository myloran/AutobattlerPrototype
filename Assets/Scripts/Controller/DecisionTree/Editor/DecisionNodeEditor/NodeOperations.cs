using System.Collections.Generic;
using Controller.DecisionTree.Nodes;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Controller.DecisionTree.Editor.DecisionNodeEditor {
  public class NodeOperations {
    public NodeOperations(DecisionNode target) {
      this.target = target;
    }

    public void RemoveNodes(List<Node> nodes, NodeGraphEditor graphEditor) {
      foreach (var node in nodes) {
        if (graphEditor.CanRemove(node))
          graphEditor.RemoveNode(node);
      }
    }

    public List<Node> CopyNodes() {
      var nodes = new List<Node> {target};
      AddChildNodes(target, nodes);
      return nodes;
    }

    public void PasteNodes(List<Node> nodes) {
      var previousCopyBuffer = NodeEditorWindow.copyBuffer;
      NodeEditorWindow.copyBuffer = nodes.ToArray();
      NodeEditorWindow.current.PasteNodes(target.position + new Vector2(100, 100));
      NodeEditorWindow.copyBuffer = previousCopyBuffer;
    }
		
    void AddChildNodes(Node node, List<Node> nodes) {
      foreach (var port in node.Outputs) {
        var connection = port.Connection;
        if (connection == null) {
          Debug.LogError($"Missing connection");
          continue;
        }
		
        nodes.Add(connection.node);
        AddChildNodes(connection.node, nodes);
      }
    }

    readonly DecisionNode target;
  }
}