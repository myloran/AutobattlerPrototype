using Controller.DecisionTree.Data;

namespace Controller.DecisionTree.Visitor {
  public interface IVisitor<out T> {
    T VisitDecision(DecisionData data);
    T VisitAction(ActionData data);
  }
}