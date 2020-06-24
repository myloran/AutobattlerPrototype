using Model.NAI.NDecisionTree;
using Model.NUnit.Abstraction;

namespace Model.NUnit {
  public interface IDecisionTreeFactory {
    IDecisionTreeNode Create(IUnit unit);
  }
}