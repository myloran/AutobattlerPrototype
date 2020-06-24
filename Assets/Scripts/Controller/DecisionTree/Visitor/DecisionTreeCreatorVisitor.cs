using System;
using System.Collections.Generic;
using System.Linq;
using Controller.DecisionTree.Data;
using Model.NAI.NDecisionTree;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Controller.DecisionTree.Visitor {
  //TODO: move functionality to composite/refactor
  public class DecisionTreeCreatorVisitor : IVisitor<IDecisionTreeNode> {
    public IDecisionTreeNode VisitDecision(DecisionData data) {
      if (data.Components.Count != 2) throw new Exception($"decision: {data.Type}");
      
      var type = decisionTypes[data.Type];
      var decision = (BaseDecision)Activator.CreateInstance(type);
      var onTrue = data.Components[0].Accept(this);
      var onFalse = data.Components[1].Accept(this);
      decision.Do(onTrue).Else(onFalse);
      decision.Init(onTrue, onFalse, unit);

      return log(decision);
    }

    public IDecisionTreeNode VisitAction(ActionData data) {
      var type = decisionTypes[data.Type];
      var action = (BaseAction)Activator.CreateInstance(type);
      action.Init(unit, bus);
      return log(action);
    }

    public void Init() => decisionTypes = LookupDecisionTypes(typeof(IDecisionTreeNode),
      typeof(BaseAction), typeof(BaseDecision));


    public IDecisionTreeNode Create(DecisionTreeComponent component, IUnit unit, IEventBus bus,
        Func<IDecisionTreeNode, IDecisionTreeNode> log) {
      this.log = log;
      this.bus = bus;
      this.unit = unit;
      return component.Accept(this);
    }

    //TODO: check if list contains interfaces/abstract classes
    Dictionary<EDecision, Type> LookupDecisionTypes(Type type, params Type[] typesExcluded) =>
      AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(s => s.GetTypes())
        .Where(type.IsAssignableFrom)
        .Where(t => t != type && typesExcluded.Select(t2 => t != t2).All(t2 => t2))
        .Select(t => (t, (IDecisionTreeNode)Activator.CreateInstance(t)))
        .ToDictionary(n => n.Item2.Type, n => n.t);
    
    Dictionary<EDecision, Type> decisionTypes = new Dictionary<EDecision, Type>();
    IUnit unit;
    IEventBus bus;
    Func<IDecisionTreeNode, IDecisionTreeNode> log;
  }
}
