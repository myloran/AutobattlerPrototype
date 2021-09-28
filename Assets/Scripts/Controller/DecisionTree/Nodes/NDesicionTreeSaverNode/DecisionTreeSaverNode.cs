using Controller.DecisionTree.Data;
using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
  public class DecisionTreeSaverNode : Node {
    [Output, HideInInspector] public bool Output;

    public void Load() => loader.Load(this, dataLoader);
    public void Save() => saver.Save(this, dataLoader);
    
    
    public override object GetValue(NodePort port) {
      return null; // Replace this
    }

    readonly DecisionTreeDataLoader dataLoader = new DecisionTreeDataLoader();
    readonly DecisionTreeLoader loader = new DecisionTreeLoader();
    readonly DecisionTreeSaver saver = new DecisionTreeSaver();
    DecisionTreeGraph decisionTreeGraph;
  }
}