using Controller.DecisionTree.Visitor;
using MessagePack;

namespace Controller.DecisionTree.Data {
  [Union(0, typeof(ActionData))]
  [Union(1, typeof(DecisionData))]
  public interface IDecisionTreeComponent {
    T Accept<T>(IDecisionTreeDataVisitor<T> decisionTreeDataVisitor);
  }
}