using System;
using System.Linq;
using Controller.DecisionTree.Data;
using Model.NAI.NDecisionTree;
using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
  public class DecisionTreeLoader {
    public void Load(DecisionTreeSaverNode node, DecisionTreeDataLoader dataLoader) {
      var decisionTreeGraph = node.graph as DecisionTreeGraph;
      var component = dataLoader.Load();
      var firstNode = node.GetPort(nameof(node.Output)).Connection.node;
      Debug.Log($"Load: {component}");
      LoadComponent(firstNode, component, decisionTreeGraph);
    }

    void LoadComponent(Node node, DecisionTreeComponent component, DecisionTreeGraph decisionTreeGraph) {
      if (!node.Outputs.Any()) { //TODO: add a button force reload and recreate node if missing
        SetNodeTypeId(decisionTreeGraph.ActionTypeIds);
        return;
      }
      
      SetNodeTypeId(decisionTreeGraph.DecisionTypeIds);

      var decision = component as DecisionData;
      var decisionNode = node as DecisionNode;
      if (decisionNode == null) {
        Debug.LogError($"DecisionNode is null");
        return;
      }
      LoadComponentFromPort(nameof(decisionNode.Output1), decision.OnTrue);
      LoadComponentFromPort(nameof(decisionNode.Output2), decision.OnFalse);
      
      void SetNodeTypeId(int[] ids) {
        var type = 0;
        
        for (int i = 0; i < ids.Length; i++) {
          if (ids[i] != (int) component.Type) continue;

          type = i;
          break;
        }

        if (node is IDecisionTreeNodeType decisionTreeNode)
          decisionTreeNode.TypeId = type;
      }
      
      void LoadComponentFromPort(string name, DecisionTreeComponent decisionComponent) {
        var port = node.GetPort(name);
        var connectionNode = SelectConnectionNode(port, component.Type);
        LoadComponent(connectionNode, decisionComponent, decisionTreeGraph);
      }
      
      
      Node SelectConnectionNode(NodePort port, EDecisionTreeType type2) {
        if (port.ConnectionCount == 0) throw new Exception($"Decision {type2} does not have connection");
        return port.Connection.node;
      }
    }
  }
}