using Controller.DecisionTree.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Controller.DecisionTree.Editor {
  [CustomNodeEditor(typeof(ActionNode))]
  public class ActionNodeEditor : NodeEditor {
    DecisionNode node;

    public override void OnBodyGUI() {
      if (target == null) {
        Debug.LogWarning("Null target node for node editor!");
        return;
      }

      NodePort input = target.GetPort("input");

      GUILayout.BeginHorizontal();
        if (input != null) NodeEditorGUILayout.PortField(GUIContent.none, input, GUILayout.MinWidth(0));
        
        var node = target as ActionNode;
        var graph = node.graph as DecisionTreeGraph;
        node.Selected = EditorGUILayout.Popup(node.Selected, graph.ActionTypes, GUILayout.Width(150));
      GUILayout.EndHorizontal();
      
      EditorGUIUtility.labelWidth = 60;
      
      base.OnBodyGUI();
    }
  }
}