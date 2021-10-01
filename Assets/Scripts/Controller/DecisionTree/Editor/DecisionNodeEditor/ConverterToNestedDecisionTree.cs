using Controller.DecisionTree.Nodes;
using XNodeEditor;

namespace Controller.DecisionTree.Editor.DecisionNodeEditor {
  public class ConverterToNestedDecisionTree {
	  public ConverterToNestedDecisionTree(DecisionNode target) {
		  nestedGraphAssetPath = new NestedGraphAssetPath(target);
		  nodeOperations = new NodeOperations(target);
		  nestedNodeCreator = new NestedNodeCreator(target);
		  nestedDecisionTreeGraphCreator = new NestedDecisionTreeGraphCreator(target, nodeOperations);
	  }
	  
		public void ConvertToNestedDecisionTree() {
			if (!nestedGraphAssetPath.TryGetNestedGraphAssetPath(out var assetPath)) return;
			
			var nestedGraph = nestedDecisionTreeGraphCreator.CreateNestedDecisionTreeGraph();
			nestedNodeCreator.CreateNestedDecisionTreeNode(nestedGraph);
			nestedDecisionTreeGraphCreator.SaveNestedGraphToAssetDatabase(nestedGraph, assetPath);
		}
		
		public void ConvertToExistingDecisionTree() {
			var nodes = nodeOperations.CopyNodes();
			nestedNodeCreator.CreateNestedDecisionTreeNode(null);
			nodeOperations.RemoveNodes(nodes, NodeEditorWindow.current.graphEditor);
		}

		readonly NestedGraphAssetPath nestedGraphAssetPath;
		readonly NodeOperations nodeOperations;
		readonly NestedNodeCreator nestedNodeCreator;
		readonly NestedDecisionTreeGraphCreator nestedDecisionTreeGraphCreator;
  }
}