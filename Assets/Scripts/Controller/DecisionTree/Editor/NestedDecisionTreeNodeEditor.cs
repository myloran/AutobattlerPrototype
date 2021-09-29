using Controller.DecisionTree.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Controller.DecisionTree.Editor {
  [CustomNodeEditor(typeof(NestedDecisionTreeNode))]
  public class NestedDecisionTreeNodeEditor : NodeEditor {
    DecisionNode node;

    public override void OnBodyGUI() {
      if (target == null) {
        Debug.LogWarning("Null target node for node editor!");
        return;
      }

      var node = target as NestedDecisionTreeNode;
      NodePort input = target.GetPort(nameof(node.Input));

      GUILayout.BeginHorizontal();
      
        if (input != null) NodeEditorGUILayout.PortField(GUIContent.none, input, GUILayout.MinWidth(0));
        
      GUILayout.EndHorizontal();
      
      EditorGUIUtility.labelWidth = 60;
      
      base.OnBodyGUI();
    }
  }
}