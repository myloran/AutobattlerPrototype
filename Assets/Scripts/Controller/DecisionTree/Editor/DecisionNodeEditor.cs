using System.Collections.Generic;
using System.IO;
using System.Linq;
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
      NodePort input = target.GetPort("Input");
      NodePort output1 = target.GetPort("Output1");
      NodePort output2 = target.GetPort("Output2");

      GUILayout.BeginHorizontal();
        if (input != null) NodeEditorGUILayout.PortField(GUIContent.none, input, GUILayout.MinWidth(0));
        
        var node = target as DecisionNode;
        var graph = node.graph as DecisionTreeGraph;
        
        EditorGUI.BeginChangeCheck();
        var nodeTypeId = EditorGUILayout.Popup(node.TypeId, graph.DecisionTypeNames, GUILayout.Width(150));
        
        if (EditorGUI.EndChangeCheck()) {
          Undo.RecordObject(target, "Changed decision type");
          node.TypeId = nodeTypeId;
          // Debug.Log(_options[_selected]);
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
			var graph = target.graph;
			var graphAssetPath = AssetDatabase.GetAssetPath(graph);
			var graphFolderPath = Path.GetDirectoryName(graphAssetPath);
			
			if (graphFolderPath == null) {
				Debug.LogError($"{nameof(graphFolderPath)} is null");
				return;
			}
			
			var nestedGraphFolderPath = Path.Combine(graphFolderPath, graph.name);
			if (!Directory.Exists(nestedGraphFolderPath)) Directory.CreateDirectory(nestedGraphFolderPath);
			
			var decisionTreeGraph = graph as DecisionTreeGraph;
			if (decisionTreeGraph == null) {
				Debug.LogError($"Can only convert inside DecisionTreeGraph");
				return;
			}

			var decision = target as DecisionNode;
			if (decisionTreeGraph.DecisionTypeNames.Length < decision.TypeId) {
				Debug.LogError($"{nameof(decision.TypeId)} is missing inside {nameof(decisionTreeGraph.DecisionTypeNames)}");
				return;
			}
			
			var decisionTypeName = decisionTreeGraph.DecisionTypeNames[decision.TypeId];
			var nestedGraphName = decisionTypeName + ".asset";
			var nestedGraphAssetPath = Path.Combine(nestedGraphFolderPath, nestedGraphName);
			var assetPath = AssetDatabase.GenerateUniqueAssetPath(nestedGraphAssetPath);
			var nestedGraph = ScriptableObject.CreateInstance<DecisionTreeGraph>();

			var nodes = CopyNodes();
			AssetDatabase.CreateAsset(nestedGraph, assetPath);
			AssetDatabase.SaveAssets();
			// AssetDatabase.OpenAsset(nestedGraph);
			//add required nodes to nested graph
			//removed them from the current one
			var window = NodeEditorWindow.Open(nestedGraph);
			nestedGraph.OnInit += () => PasteNodes(nodes); 
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = nestedGraph;
			Debug.Log($"nestedGraphAssetPath: {nestedGraphAssetPath}");
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