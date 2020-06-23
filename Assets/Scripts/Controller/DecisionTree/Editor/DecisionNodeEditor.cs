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
        EditorGUI.BeginChangeCheck();
        if (input != null) NodeEditorGUILayout.PortField(GUIContent.none, input, GUILayout.MinWidth(0));
        
        var node = target as DecisionNode;
        var graph = node.graph as DecisionTreeGraph;
        node.Selected = EditorGUILayout.Popup(node.Selected, graph.DecisionTypes, GUILayout.Width(150));
        
        if (EditorGUI.EndChangeCheck()) {
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
  }
}