using Controller.DecisionTree.Editor.DecisionNodeEditor;
using Controller.DecisionTree.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Controller.DecisionTree.Editor {
  [CustomNodeEditor(typeof(DecisionNode))]
  public class DecisionTreeNodeEditor : NodeEditor {
	  public override void OnCreate() {
		  base.OnCreate();
      converter = new ConverterToNestedDecisionTree(target as DecisionNode);
      nodeOperations = new NodeOperations(target);
	  }

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
	    menu.AddItem(new GUIContent("Convert To New Nested Decision Tree"), false, converter.ConvertToNewNestedDecisionTree);
	    menu.AddItem(new GUIContent("Convert To Existing Nested Decision Tree"), false, converter.ConvertToExistingDecisionTree);
      menu.AddItem(new GUIContent("Save"), false, Save);
    }

    void Save() {
      var saveNode = nodeHelper.TrySearchInInputsRecursively<DecisionTreeSaverNode>(target);
      if (saveNode != null) 
        saveNode.Save();
      else 
        IfNotFound();
    }

    void IfNotFound() => EditorUtility.DisplayDialog("Save", "DecisionTreeSaverNode is not found", "Ok");

    ConverterToNestedDecisionTree converter;
    NodeOperations nodeOperations;
    readonly NodeHelper nodeHelper = new NodeHelper();
  }
}