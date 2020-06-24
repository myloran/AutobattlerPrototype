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
      // Debug.Log($"before: {component}");
      loader.Save(component);
      var c2 = loader.Load();
      // Debug.Log($"after: {c2}");
    }

    public DecisionTreeComponent CreateComponent(Node node) {
      if (!node.Outputs.Any()) return CreateAction(node);

      var typeNode = node as IDecisionTreeTypeNode;
      var type = (EDecision) decisionTreeGraph.DecisionIds[typeNode.Selected];
      // Debug.Log($"decision: {type}");
      var decisionData = new DecisionData(type);
      
      var components = node.Outputs
        .Select(o => SelectConnectionNode(o, type))
        .Select(CreateComponent);
      
      decisionData.AddRange(components);
      return decisionData;
    }

    DecisionTreeComponent CreateAction(Node node) {
      var typeNode = node as IDecisionTreeTypeNode;
      var type = (EDecision) decisionTreeGraph.ActionIds[typeNode.Selected];
      // Debug.Log($"action: {type}");
      return new ActionData(type);
    }

    static Node SelectConnectionNode(NodePort o, EDecision type2) {
      if (o.ConnectionCount == 0) throw new Exception($"Decision {type2} does not have connection");
      return o.Connection.node;
    }

    readonly DecisionTreeLoader loader = new DecisionTreeLoader();
    DecisionTreeGraph decisionTreeGraph;

    // firstNode.AcceptVisitor(this);
      // GetDecision(firstNode);
      //
      // EDecision GetDecision(Node node) {
      //   if (!node.Outputs.Any()) {
      //     var typeNode = node as IDecisionTreeTypeNode;
      //     var type = (EDecision) decisionTreeGraph.ActionIds[typeNode.Selected];
      //     return new ActionData(type);
      //   } 
      //     
      //   foreach (var o in node.Outputs) {
      //     var node2 = o.Connection.node;
      //     var typeNode2 = node2 as IDecisionTreeTypeNode;
      //   }

      // var node = GetPort("output").Connection.node;
      // var typeNode = node as IDecisionTreeTypeNode;
      //
      // if (typeNode.Type == EDecision.BaseAction) {
      //   var type = (EDecision) decisionTreeGraph.ActionIds[typeNode.Selected];
      // var action = new ActionData(type);
      // }
      // else {
      //   foreach (var o in node.Outputs) {
      //     var node2 = o.Connection.node;
      //     var typeNode2 = node2 as IDecisionTreeTypeNode;
      //   }
      // }
      // }
    

    public override object GetValue(NodePort port) {
      return null; // Replace this
    }
  }
}