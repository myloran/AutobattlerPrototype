using System;
using XNodeEditor;

namespace Controller.DecisionTree.Editor {
  [CustomNodeGraphEditor(typeof(DecisionTreeGraph))]
  public class DecisionTreeGraphEditor : NodeGraphEditor{
    public override string GetNodeMenuName(Type type) =>
      type.Namespace == "Controller.DecisionTree.Nodes" 
        ? base.GetNodeMenuName(type).Replace("Controller/Decision Tree/Nodes/", "") 
        : null;

    public override void OnOpen() {
      var graph = target as DecisionTreeGraph;
      graph.Init();
    }
  }
}