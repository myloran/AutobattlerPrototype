using System;
using Controller.DecisionTree.Data;
using Controller.DecisionTree.Visitor;
using Model.NAI.NDecisionTree;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Controller {
  public class DecisionFactory : IDecisionTreeFactory {
    public DecisionFactory(IEventBus bus, Func<IDecisionTreeNode, IDecisionTreeNode> log, 
        DecisionTreeCreatorVisitor decisionTreeCreator, IDecisionTreeComponent component) {
      this.bus = bus;
      this.log = log;
      this.decisionTreeCreator = decisionTreeCreator;
      this.component = component;
    }

    public IDecisionTreeNode Create(IUnit unit) { //TODO: replace ienumerable in model to avoid allocations 
      //TODO: Think of mechanism to not queue MakeDecision command and instead subscribe to interested events and make decision when something happens
       if (decisionTree == null) 
         decisionTree = decisionTreeCreator.Create(component, unit, bus, log);

       var clone = decisionTree.Clone();
       clone.SetUnit(unit);
       return clone;
    }
    
    readonly IEventBus bus;
    readonly Func<IDecisionTreeNode, IDecisionTreeNode> log;
    readonly DecisionTreeCreatorVisitor decisionTreeCreator;
    readonly IDecisionTreeComponent component;
    IDecisionTreeNode decisionTree;
  }
}