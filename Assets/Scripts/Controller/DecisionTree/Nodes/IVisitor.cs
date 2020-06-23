using Controller.DecisionTree.Data;

namespace Controller.DecisionTree.Nodes {
  public interface IVisitor<T> {
    T VisitDecision(DecisionData decision, T state);
    T VisitAction(ActionData action, T state);
  }
}