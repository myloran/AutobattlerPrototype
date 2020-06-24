using System;
using System.Collections.Generic;
using System.Linq;
using Model.NAI.NDecisionTree;

namespace Model {
  public class DecisionTreeLookup {
    //TODO: check if list contains interfaces/abstract classes
    public Dictionary<EDecision, Type> LookupDecisionTypes() =>
      LookupDecisionTypes(typeof(IDecisionTreeNode), typeof(BaseAction), typeof(BaseDecision));
    
    public Dictionary<EDecision, Type> LookupDecisionTypes(Type type, params Type[] typesExcluded) =>
      AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(s => s.GetTypes())
        .Where(type.IsAssignableFrom)
        .Where(t => t != type && typesExcluded.Select(t2 => t != t2).All(t2 => t2))
        .Select(t => (t, (IDecisionTreeNode)Activator.CreateInstance(t)))
        .ToDictionary(n => n.Item2.Type, n => n.t);
  }
}