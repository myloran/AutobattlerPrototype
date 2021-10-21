using System.Linq;
using Controller.DecisionTree.Nodes;
using XNode;

namespace Controller.DecisionTree {
  public class NodeHelper {
    public T TrySearchInInputsRecursively<T>(Node node) where T : class {
      if (node.Inputs.Any()) {
        var inputNode = node.Inputs.First().Connection.node;
        
        return inputNode.GetType() == typeof(T) 
          ? inputNode as T 
          : TrySearchInInputsRecursively<T>(inputNode);
      }
      
      if (node.GetType() == typeof(T)) return node as T;
      
      if (node is ParentDecisionTreeNode parentNode) {
        foreach (var n in parentNode.Graph.nodes) {
          if (n is NestedDecisionTreeNode nestedNode) {
            var result = TrySearchInInputsRecursively<T>(nestedNode);
            if (result != null) return result;
          }
        }
      }
      
      return null;
    }
  }
}