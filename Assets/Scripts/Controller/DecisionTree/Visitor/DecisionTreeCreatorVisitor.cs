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
      if (data.Components.Count != 2) throw new Exception($"decision: {data.Type}");
      
      var type = decisionTypes[data.Type];
      var decision = (BaseDecision)Activator.CreateInstance(type);
      var onTrue = data.Components[0].Accept(this);
      var onFalse = data.Components[1].Accept(this);
      decision.Init(onTrue, onFalse);

      return log(decision);
    }

    public IDecisionTreeNode VisitAction(ActionData data) {
      var type = decisionTypes[data.Type];
      var action = (BaseAction)Activator.CreateInstance(type);
      action.Init(bus);
      return log(action);
    }

    Dictionary<EDecision, Type> decisionTypes = new Dictionary<EDecision, Type>();
    readonly IEventBus bus;
    readonly Func<IDecisionTreeNode, IDecisionTreeNode> log;
    readonly DecisionTreeLookup lookup;
  }
}
