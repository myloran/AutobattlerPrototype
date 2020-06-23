using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Data.DecisionTree.Nodes.Editor {
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
      GUILayout.EndHorizontal();
      EditorGUIUtility.labelWidth = 60;
      base.OnBodyGUI();
    }
  }
}