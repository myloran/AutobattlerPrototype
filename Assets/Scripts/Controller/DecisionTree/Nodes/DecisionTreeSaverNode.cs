using System;
using System.Linq;
using Controller.DecisionTree.Data;
using Model.NAI.NDecisionTree;
using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
  public class DecisionTreeSaverNode : Node {
    [Output, HideInInspector] public bool output;

    public void Save() {
      decisionTreeGraph = graph as DecisionTreeGraph;

      var firstNode = GetPort("output").Connection.node;
      var component = CreateComponent(firstNode);
      Debug.Log($"before: {component}");
      loader.Save(component);
    }

    DecisionTreeComponent CreateComponent(Node node) {
      if (!node.Outputs.Any()) return CreateAction();

      var decisionNode = node as IDecisionTreeNodeType;
      var decisionType = (EDecisionTreeType) decisionTreeGraph.DecisionTypeIds[decisionNode.TypeId];
      
      return new DecisionData {
        Type = decisionType,
        OnTrue = CreateComponentFromPort("Output1"),
        OnFalse = CreateComponentFromPort("Output2")
      };
     
      DecisionTreeComponent CreateAction() {
        var action = node as IDecisionTreeNodeType;
        var actionType = (EDecisionTreeType) decisionTreeGraph.ActionTypeIds[action.TypeId];
        return new ActionData {Type = actionType};
      }
      
      DecisionTreeComponent CreateComponentFromPort(string name) {
        var port = node.GetPort(name);
        var connectionNode = SelectConnectionNode(port, decisionType);
        return CreateComponent(connectionNode);
      }
    }

    Node SelectConnectionNode(NodePort port, EDecisionTreeType type2) {
      if (port.ConnectionCount == 0) throw new Exception($"Decision {type2} does not have connection");
      return port.Connection.node;
    }

    public override object GetValue(NodePort port) {
      return null; // Replace this
    }

    public void Load() {
      decisionTreeGraph = graph as DecisionTreeGraph;
      
      var component = loader.Load();
      Debug.Log($"Load: {component}");
      var firstNode = GetPort("output").Connection.node;
      LoadComponent(firstNode, component);
    }

    void LoadComponent(Node node, DecisionTreeComponent component) {
      if (!node.Outputs.Any()) { //TODO: add a button force reload and recreate node if missing
        SetNodeTypeId(decisionTreeGraph.ActionTypeIds);
        return;
      }
      
      SetNodeTypeId(decisionTreeGraph.DecisionTypeIds);

      var decision = component as DecisionData;
      LoadComponentFromPort("Output1", decision.OnTrue);
      LoadComponentFromPort("Output2", decision.OnFalse);
      
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
        LoadComponent(connectionNode, decisionComponent);
      }
    }

    readonly DecisionTreeLoader loader = new DecisionTreeLoader();
    DecisionTreeGraph decisionTreeGraph;
  }
}