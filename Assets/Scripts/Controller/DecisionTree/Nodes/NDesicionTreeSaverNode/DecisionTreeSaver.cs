using System;
using System.Linq;
using Controller.DecisionTree.Data;
using Model.NAI.NDecisionTree;
using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
  public class DecisionTreeSaver {
    public void Save(DecisionTreeSaverNode node, DecisionTreeDataLoader dataLoader) {
      var decisionTreeGraph = node.graph as DecisionTreeGraph;
      var firstNode = node.GetPort(nameof(node.Output)).Connection.node;
      var component = CreateComponent(firstNode, decisionTreeGraph);
      Debug.Log($"before: {component}");
      dataLoader.Save(component);
    }

    DecisionTreeComponent CreateComponent(Node node, DecisionTreeGraph decisionTreeGraph) {
      if (!node.Outputs.Any()) {
        if (node is NestedDecisionTreeNode nestedNode) {
          var parent = (ParentDecisionTreeNode)nestedNode.Graph.nodes.FirstOrDefault(n => n is ParentDecisionTreeNode);
          if (parent == null) {
            throw new Exception($"ParentDecisionTreeNode is missing in graph: {nestedNode.graph.name}");
          }

          var port = parent.GetOutputPort(nameof(parent.Output));
          return CreateComponent(port.Connection.node, decisionTreeGraph);
        }

        return CreateAction();
      }

      var decisionNode = node as DecisionNode;
      var decisionType = (EDecisionTreeType) decisionTreeGraph.DecisionTypeIds[decisionNode.TypeId];
      
      return new DecisionData {
        Type = decisionType,
        OnTrue = CreateComponentFromPort(nameof(decisionNode.Output1)),
        OnFalse = CreateComponentFromPort(nameof(decisionNode.Output2))
      };
     
      DecisionTreeComponent CreateAction() {
        var action = node as IDecisionTreeNodeType;
        var actionType = (EDecisionTreeType) decisionTreeGraph.ActionTypeIds[action.TypeId];
        return new ActionData {Type = actionType};
      }
      
      DecisionTreeComponent CreateComponentFromPort(string name) {
        var port = node.GetPort(name);
        var connectionNode = SelectConnectionNode(port, decisionType);
        return CreateComponent(connectionNode, decisionTreeGraph);
      }
    }

    Node SelectConnectionNode(NodePort port, EDecisionTreeType type2) {
      if (port.ConnectionCount == 0) throw new Exception($"Decision {type2} does not have connection");
      return port.Connection.node;
    }
  }
}