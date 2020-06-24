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
      if (!node.Outputs.Any()) return CreateAction(node);

      var typeNode = node as ISelected;
      var type = (EDecision) decisionTreeGraph.DecisionIds[typeNode.Selected];
      // Debug.Log($"decision: {type}");
      var decisionData = new DecisionData {Type = type};

      var onTrue = CreateComponentFromPort(node, type, "Output1");
      var onFalse = CreateComponentFromPort(node, type, "Output2");
      
      decisionData.AddRange(new[] {onTrue, onFalse});
      return decisionData;
    }

    DecisionTreeComponent CreateComponentFromPort(Node node, EDecision type, string name) {
      var port = node.GetPort(name);
      var connectionNode = SelectConnectionNode(port, type);
      return CreateComponent(connectionNode);
    }

    DecisionTreeComponent CreateAction(Node node) {
      var typeNode = node as ISelected;
      var type = (EDecision) decisionTreeGraph.ActionIds[typeNode.Selected];
      // Debug.Log($"action: {type}");
      return new ActionData {Type = type};
    }

    static Node SelectConnectionNode(NodePort port, EDecision type2) {
      if (port.ConnectionCount == 0) throw new Exception($"Decision {type2} does not have connection");
      return port.Connection.node;
    }

    readonly DecisionTreeLoader loader = new DecisionTreeLoader();
    DecisionTreeGraph decisionTreeGraph;

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
      LoadComponentFromPort(node, component.Type, "Output1", decision.Components[0]);
      LoadComponentFromPort(node, component.Type, "Output2", decision.Components[1]);
      
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
    }


    void LoadComponentFromPort(Node node, EDecision type, string name,
        DecisionTreeComponent decisionComponent) {
      var port = node.GetPort(name);
      var connectionNode = SelectConnectionNode(port, type);
      LoadComponent(connectionNode, decisionComponent);
    }
  }
}