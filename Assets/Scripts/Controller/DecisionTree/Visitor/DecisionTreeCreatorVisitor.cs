using System;
using System.Collections.Generic;
using Controller.DecisionTree.Data;
using Model;
using Model.NAI.NDecisionTree;
using PlasticFloor.EventBus;

namespace Controller.DecisionTree.Visitor {
  //TODO: refactor
  public class DecisionTreeCreatorVisitor : IDecisionTreeDataVisitor<IDecisionTreeNode> {
    public DecisionTreeCreatorVisitor(IEventBus bus,
        Func<IDecisionTreeNode, IDecisionTreeNode> log, DecisionTreeLookup lookup) {
      this.log = log;
      this.lookup = lookup;
      this.bus = bus;
    }
    
    public void Init() => decisionTypes = lookup.LookupDecisionTypes();
    
    public IDecisionTreeNode VisitDecision(DecisionData data) {
      var type = decisionTypes[data.Type];
      var decision = (BaseDecision)Activator.CreateInstance(type);
      var onTrue = data.OnTrue.Accept(this);
      var onFalse = data.OnFalse.Accept(this);
      decision.Init(onTrue, onFalse);

      return log(decision);
    }

    public IDecisionTreeNode VisitAction(ActionData data) {
      var type = decisionTypes[data.Type];
      var action = (BaseAction)Activator.CreateInstance(type);
      action.Init(bus);
      return log(action);
    }

    Dictionary<EDecisionTreeType, Type> decisionTypes = new Dictionary<EDecisionTreeType, Type>();
    readonly IEventBus bus;
    readonly Func<IDecisionTreeNode, IDecisionTreeNode> log;
    readonly DecisionTreeLookup lookup;
  }
}
