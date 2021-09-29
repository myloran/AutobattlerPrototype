using Controller.DecisionTree.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Controller.DecisionTree.Editor {
  [CustomNodeEditor(typeof(ParentDecisionTreeNode))]
  public class ParentDecisionTreeNodeEditor : NodeEditor {
    DecisionNode node;

    public override void OnBodyGUI() {
      if (target == null) {
        Debug.LogWarning("Null target node for node editor!");
        return;
      }

      var node = target as ParentDecisionTreeNode;
      NodePort output = target.GetPort(nameof(node.Output));

      GUILayout.BeginHorizontal();
      
      if (output != null) NodeEditorGUILayout.PortField(GUIContent.none, output, GUILayout.MinWidth(0));
        
      GUILayout.EndHorizontal();
      
      EditorGUIUtility.labelWidth = 60;
      
      base.OnBodyGUI();
    }
  }
}