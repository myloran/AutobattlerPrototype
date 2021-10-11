using System;
using System.Collections.Generic;
using System.Linq;
using Controller.DecisionTree.Nodes;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Controller.DecisionTree.Editor.DecisionNodeEditor {
  public class NodeOperations {
    public NodeOperations(Node target) {
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

    public T TrySearchInInputsRecursively<T>(Node node) where T : class {
      if (node.Inputs.Any()) {
        var inputNode = node.Inputs.First().Connection.node;
        
        return inputNode.GetType() == typeof(T) 
          ? inputNode as T 
          : TrySearchInInputsRecursively<T>(inputNode);
      }
      
      if (node.GetType() == typeof(T)) return node as T;
      
      if (node is ParentDecisionTreeNode parentNode) {
        foreach (var n in parentNode.Graph.nodes) {
          if (n is NestedDecisionTreeNode nestedNode) {
            var result = TrySearchInInputsRecursively<T>(nestedNode);
            if (result != null) return result;
          }
        }
      }
      
      return null;
    }

    readonly Node target;
  }
}