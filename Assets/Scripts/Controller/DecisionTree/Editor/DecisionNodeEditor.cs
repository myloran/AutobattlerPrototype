using System.Collections.Generic;
using System.IO;
using System.Linq;
using Controller.DecisionTree.Editor.Exts;
using Controller.DecisionTree.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Controller.DecisionTree.Editor {
  [CustomNodeEditor(typeof(DecisionNode))]
  public class DecisionTreeNodeEditor : NodeEditor {
    public override void OnBodyGUI() {
      if (target == null) {
        Debug.LogWarning("Null target node for node editor!");
        return;
      }
      
      var node = target as DecisionNode;
      NodePort input = target.GetPort(nameof(node.Input));
      NodePort output1 = target.GetPort(nameof(node.Output1));
      NodePort output2 = target.GetPort(nameof(node.Output2));

      GUILayout.BeginHorizontal();
        if (input != null) NodeEditorGUILayout.PortField(GUIContent.none, input, GUILayout.MinWidth(0));
        
        var graph = node.graph as DecisionTreeGraph;
        
        EditorGUI.BeginChangeCheck();
        var nodeTypeId = EditorGUILayout.Popup(node.TypeId, graph.DecisionTypeNames, GUILayout.Width(150));
        
        if (EditorGUI.EndChangeCheck()) {
          Undo.RecordObject(target, "Changed decision type");
          node.TypeId = nodeTypeId;
        }
        
        if (output1 != null) NodeEditorGUILayout.PortField(GUIContent.none, output1, GUILayout.MinWidth(0));
      GUILayout.EndHorizontal();
      
      GUILayout.BeginHorizontal();
        if (output2 != null) NodeEditorGUILayout.PortField(GUIContent.none, output2, GUILayout.MinWidth(0));
      GUILayout.EndHorizontal();
      
      EditorGUIUtility.labelWidth = 60;

      base.OnBodyGUI();
    }

    public override void AddContextMenuItems(GenericMenu menu) {
	    base.AddContextMenuItems(menu);
	    menu.AddItem(new GUIContent("Convert To Nested Decision Tree"), false, ConvertToNestedDecisionTree);
    }

		void ConvertToNestedDecisionTree() {
			if (!TryGetNestedGraphAssetPath(out var nestedGraphAssetPath)) return;
			
			var assetPath = AssetDatabase.GenerateUniqueAssetPath(nestedGraphAssetPath);
			var nestedGraph = ScriptableObject.CreateInstance<DecisionTreeGraph>();
			var nodes = CopyNodes();
			var currentGraphEditor = NodeEditorWindow.current.graphEditor;
			nestedGraph.OnInit += () => {
				PasteNodes(nodes);
				RemoveNodes(nodes, currentGraphEditor);
				LinkToParentDecisionTree(target.graph as DecisionTreeGraph);
			};
			
			var node = (NestedDecisionTreeNode)NodeEditorWindow.current.graphEditor.CreateNode(typeof(NestedDecisionTreeNode), target.position);
			node.Graph = nestedGraph;
			var port = node.GetPort(nameof(node.Input));
			var otherPort = DisconnectCurrentNode();
			port.Connect(otherPort);
			
			AssetDatabase.CreateAsset(nestedGraph, assetPath);
			AssetDatabase.SaveAssets();
			NodeEditorWindow.Open(nestedGraph);
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = nestedGraph;
		}

		void LinkToParentDecisionTree(DecisionTreeGraph targetGraph) {
			var decisionNode = (DecisionNode)NodeEditorWindow.current.graph.nodes.MinBy(n => n.position.x);
			var decisionPort = decisionNode.GetInputPort(nameof(decisionNode.Input));
			var offset = new Vector2(-300, 0);
			var parentNode = (ParentDecisionTreeNode)NodeEditorWindow.current.graphEditor.CreateNode(typeof(ParentDecisionTreeNode), decisionNode.position + offset);
			var parentPort = parentNode.GetOutputPort(nameof(parentNode.Output));
			parentNode.Graph = targetGraph;
			parentPort.Connect(decisionPort);
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

		void RemoveNodes(List<Node> nodes, NodeGraphEditor graphEditor) {
			foreach (var node in nodes) {
				if (graphEditor.CanRemove(node))
					graphEditor.RemoveNode(node);
			}
		}

		bool TryGetNestedGraphAssetPath(out string nestedGraphAssetPath) {
			nestedGraphAssetPath = default;
			var graph = target.graph;
			if (!TryGetNestedGraphFolderPath(graph, out var nestedGraphFolderPath)) return false;
			if (!TryGetDecisionTypeName(graph, out var decisionTypeName)) return false;
			if (!Directory.Exists(nestedGraphFolderPath)) Directory.CreateDirectory(nestedGraphFolderPath);

			var nestedGraphName = decisionTypeName + ".asset";
			nestedGraphAssetPath = Path.Combine(nestedGraphFolderPath, nestedGraphName);
			return true;
		}

		bool TryGetNestedGraphFolderPath(NodeGraph graph, out string nestedGraphFolderPath) {
			nestedGraphFolderPath = default;
			var graphAssetPath = AssetDatabase.GetAssetPath(graph);
			var graphFolderPath = Path.GetDirectoryName(graphAssetPath);

			if (graphFolderPath == null) {
				Debug.LogError($"{nameof(graphFolderPath)} is null");
				return false;
			}

			nestedGraphFolderPath = Path.Combine(graphFolderPath, graph.name);
			return true;
		}

		bool TryGetDecisionTypeName(NodeGraph graph, out string decisionTypeName) {
			decisionTypeName = default;
			var decisionTreeGraph = graph as DecisionTreeGraph;
			if (decisionTreeGraph == null) {
				Debug.LogError($"Can only convert inside DecisionTreeGraph");
				return false;
			}

			var decision = target as DecisionNode;
			if (decisionTreeGraph.DecisionTypeNames.Length < decision.TypeId) {
				Debug.LogError($"{nameof(decision.TypeId)} is missing inside {nameof(decisionTreeGraph.DecisionTypeNames)}");
				return false;
			}

			decisionTypeName = decisionTreeGraph.DecisionTypeNames[decision.TypeId];
			return true;
		}

		List<Node> CopyNodes() {
			var nodes = new List<Node> {target};
			AddChildNodes(target, nodes);
			return nodes;
		}

		void PasteNodes(List<Node> nodes) {
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
  }
}