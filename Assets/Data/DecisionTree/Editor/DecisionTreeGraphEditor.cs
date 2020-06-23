using System;
using UnityEngine;
using XNodeEditor;

namespace Data.DecisionTree.Editor {
  [CustomNodeGraphEditor(typeof(DecisionTreeGraph))]
  public class DecisionTreeGraphEditor : NodeGraphEditor{
    public override string GetNodeMenuName(Type type) =>
      type.Namespace == "Data.DecisionTree.Nodes" 
        ? base.GetNodeMenuName(type).Replace("Data/Decision Tree/Nodes/", "") 
        : null;

    public override void OnOpen() {
      var graph = target as DecisionTreeGraph;
      graph.Init();
    }
  }
}