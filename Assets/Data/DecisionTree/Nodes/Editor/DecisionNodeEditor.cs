using Data.DecisionTree;
using Data.DecisionTree.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Data.AI.Nodes.Editor {
  [CustomNodeEditor(typeof(DecisionNode))]
  public class DecisionTreeNodeEditor : NodeEditor {
    public override void OnBodyGUI() {
      if (target == null) {
        Debug.LogWarning("Null target node for node editor!");
        return;
      }
      NodePort input = target.GetPort("input");
      NodePort output1 = target.GetPort("output1");
      NodePort output2 = target.GetPort("output2");

      GUILayout.BeginHorizontal();
      EditorGUI.BeginChangeCheck();
        if (input != null) NodeEditorGUILayout.PortField(GUIContent.none, input, GUILayout.MinWidth(0));
        
        var node = target as DecisionNode;
        var graph = node.graph as DecisionTreeGraph;
        _selected = EditorGUILayout.Popup(_selected, graph.DecisionTypes, GUILayout.Width(150));
        
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
     
    int         _selected   = 0;
  }
}