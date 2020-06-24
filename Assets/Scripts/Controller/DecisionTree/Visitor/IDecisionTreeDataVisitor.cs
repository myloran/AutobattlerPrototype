using Controller.DecisionTree.Data;

namespace Controller.DecisionTree.Visitor {
  public interface IDecisionTreeDataVisitor<out T> {
    T VisitDecision(DecisionData data);
    T VisitAction(ActionData data);
  }
}