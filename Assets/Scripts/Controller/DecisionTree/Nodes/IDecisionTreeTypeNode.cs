using Model.NAI.NDecisionTree;

namespace Controller.DecisionTree.Nodes {
  public interface IDecisionTreeTypeNode {
    EDecision Type { get; }
    int Selected { get; set; }
  }
}