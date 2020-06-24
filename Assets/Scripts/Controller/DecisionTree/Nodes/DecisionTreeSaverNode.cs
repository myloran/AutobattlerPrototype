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

      var decisionSelected = node as ISelected;
      var decisionType = (EDecision) decisionTreeGraph.DecisionIds[decisionSelected.Selected];
      
      return new DecisionData {
        Type = decisionType,
        OnTrue = CreateComponentFromPort("Output1"),
        OnFalse = CreateComponentFromPort("Output2")
      };
     
      DecisionTreeComponent CreateAction() {
        var actionSelected = node as ISelected;
        var actionType = (EDecision) decisionTreeGraph.ActionIds[actionSelected.Selected];
        return new ActionData {Type = actionType};
      }
      
      DecisionTreeComponent CreateComponentFromPort(string name) {
        var port = node.GetPort(name);
        var connectionNode = SelectConnectionNode(port, decisionType);
        return CreateComponent(connectionNode);
      }
    }

    Node SelectConnectionNode(NodePort port, EDecision type2) {
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
      if (!node.Outputs.Any()) {
        SetSelectedIndex(decisionTreeGraph.ActionIds);
        return;
      }
      
      SetSelectedIndex(decisionTreeGraph.DecisionIds);

      var decision = component as DecisionData;
      LoadComponentFromPort("Output1", decision.OnTrue);
      LoadComponentFromPort("Output2", decision.OnFalse);
      
      void SetSelectedIndex(int[] ids) {
        var selectedIndex = 0;
        
        for (int i = 0; i < ids.Length; i++) {
          if (ids[i] != (int) component.Type) continue;

          selectedIndex = i;
          break;
        }

        var selectable = node as ISelected;
        selectable.Selected = selectedIndex;
      }
      
      void LoadComponentFromPort(string name,
          DecisionTreeComponent decisionComponent) {
        var port = node.GetPort(name);
        var connectionNode = SelectConnectionNode(port, component.Type);
        LoadComponent(connectionNode, decisionComponent);
      }
    }

    readonly DecisionTreeLoader loader = new DecisionTreeLoader();
    DecisionTreeGraph decisionTreeGraph;
  }
}