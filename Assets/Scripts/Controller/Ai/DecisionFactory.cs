using Controller.DecisionTree.Data;
using Controller.DecisionTree.Visitor;
using Model.NAI.NDecisionTree;
using Model.NUnit;
using Model.NUnit.Abstraction;

namespace Controller.Ai {
  public class DecisionFactory : IDecisionTreeFactory {
    public DecisionFactory(DecisionTreeCreatorVisitor decisionTreeCreatorVisitor, 
        DecisionTreeComponent component) {
      this.decisionTreeCreatorVisitor = decisionTreeCreatorVisitor;
      this.component = component;
    }

    public IDecisionTreeNode Create(IUnit unit) { //TODO: replace ienumerable in model to avoid allocations 
      //TODO: Think of mechanism to not queue MakeDecision command and instead subscribe to interested events and make decision when something happens
       if (decisionTree == null) 
         decisionTree = component.Accept(decisionTreeCreatorVisitor);

       var clone = decisionTree.Clone();
       clone.SetUnit(unit);
       return clone;
    }
    
    readonly DecisionTreeCreatorVisitor decisionTreeCreatorVisitor;
    readonly DecisionTreeComponent component;
    IDecisionTreeNode decisionTree;
  }
}