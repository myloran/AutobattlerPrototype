using System.IO;
using Controller.DecisionTree.Nodes;
using UnityEditor;
using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Editor.DecisionNodeEditor {
  public class NestedGraphAssetPath {
    public NestedGraphAssetPath(DecisionNode target) {
      this.target = target;
    }

    public bool TryGetNestedGraphAssetPath(out string nestedGraphAssetPath) {
      nestedGraphAssetPath = default;
      var graph = target.graph;
      if (!TryGetNestedGraphFolderPath(graph, out var nestedGraphFolderPath)) return false;
      if (!TryGetDecisionTypeName(graph, out var decisionTypeName)) return false;
      if (!Directory.Exists(nestedGraphFolderPath)) Directory.CreateDirectory(nestedGraphFolderPath);

      var nestedGraphName = decisionTypeName + ".asset";
      nestedGraphAssetPath = Path.Combine(nestedGraphFolderPath, nestedGraphName);
      nestedGraphAssetPath = AssetDatabase.GenerateUniqueAssetPath(nestedGraphAssetPath);
      return true;
    }

    bool TryGetNestedGraphFolderPath(NodeGraph graph, out string nestedGraphFolderPath) {
      nestedGraphFolderPath = default;
      var graphAssetPath = AssetDatabase.GetAssetPath(graph);
      var graphFolderPath = Path.GetDirectoryName(graphAssetPath);

      if (graphFolderPath == null) {
        Debug.LogError($"{nameof(graphFolderPath)} is null");
        return false;
      }

      nestedGraphFolderPath = Path.Combine(graphFolderPath, graph.name);
      return true;
    }

    bool TryGetDecisionTypeName(NodeGraph graph, out string decisionTypeName) {
      decisionTypeName = default;
      var decisionTreeGraph = graph as DecisionTreeGraph;
      if (decisionTreeGraph == null) {
        Debug.LogError($"Can only convert inside DecisionTreeGraph");
        return false;
      }

      if (decisionTreeGraph.DecisionTypeNames.Length < target.TypeId) {
        Debug.LogError($"{nameof(target.TypeId)} is missing inside {nameof(decisionTreeGraph.DecisionTypeNames)}");
        return false;
      }

      decisionTypeName = decisionTreeGraph.DecisionTypeNames[target.TypeId];
      return true;
    }
    
    readonly DecisionNode target;
  }
}