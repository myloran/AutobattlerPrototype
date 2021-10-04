using Controller.DecisionTree.Editor.Exts;
using Controller.DecisionTree.Nodes;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Controller.DecisionTree.Editor.DecisionNodeEditor {
  public class NestedDecisionTreeGraphCreator {
    public NestedDecisionTreeGraphCreator(DecisionNode target, NodeOperations nodeOperations) {
      this.target = target;
      this.nodeOperations = nodeOperations;
    }
    
    public void SaveNestedGraphToAssetDatabase(DecisionTreeGraph nestedGraph, string assetPath) {
      AssetDatabase.CreateAsset(nestedGraph, assetPath);
      AssetDatabase.SaveAssets();
      NodeEditorWindow.Open(nestedGraph);
      EditorUtility.FocusProjectWindow();
      Selection.activeObject = nestedGraph;
    }

    public DecisionTreeGraph CreateNestedDecisionTreeGraph() {
      var nestedGraph = ScriptableObject.CreateInstance<DecisionTreeGraph>();
      var nodes = nodeOperations.CopyNodes();
      var currentGraphEditor = NodeEditorWindow.current.graphEditor;
			
      nestedGraph.OnInit += () => {
        nodeOperations.PasteNodes(nodes);
        nodeOperations.RemoveNodes(nodes, currentGraphEditor);
        LinkToParentDecisionTree(target.graph as DecisionTreeGraph);
      };
			
      return nestedGraph;
    }
		
    void LinkToParentDecisionTree(DecisionTreeGraph targetGraph) {
      var decisionNode = (DecisionNode)NodeEditorWindow.current.graph.nodes.MinBy(n => n.position.x);
      var decisionPort = decisionNode.GetInputPort(nameof(decisionNode.Input));
      var offset = new Vector2(-300, 0);
      var parentNode = (ParentDecisionTreeNode)NodeEditorWindow.current.graphEditor
        .CreateNode(typeof(ParentDecisionTreeNode), decisionNode.position + offset);
      var parentPort = parentNode.GetOutputPort(nameof(parentNode.Output));
      parentNode.Graph = targetGraph;
      parentPort.Connect(decisionPort);
    }

    readonly DecisionNode target;
    readonly NodeOperations nodeOperations;
  }
}