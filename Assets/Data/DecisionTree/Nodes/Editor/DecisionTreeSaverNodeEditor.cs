using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Data.DecisionTree.Nodes.Editor {
  [CustomNodeEditor(typeof(DecisionTreeSaverNode))]
  public class DecisionTreeNodeEditor : NodeEditor {
    public override void OnBodyGUI() {
      if (target == null) {
        Debug.LogWarning("Null target node for node editor!");
        return;
      }

      NodePort output = target.GetPort("output");

      GUILayout.BeginHorizontal();
      if (output != null) NodeEditorGUILayout.PortField(GUIContent.none, output, GUILayout.MinWidth(0));
      GUILayout.EndHorizontal();

      EditorGUIUtility.labelWidth = 60;
      
      base.OnBodyGUI();
      
      if (GUILayout.Button("Save")) {}
    }
  }
}