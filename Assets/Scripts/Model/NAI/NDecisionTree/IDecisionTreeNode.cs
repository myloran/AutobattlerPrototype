using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Newtonsoft.Json;

namespace Model.NAI.NDecisionTree {
  public interface IDecisionTreeNode {
    EDecisionTreeType Type { get; }
    [JsonIgnore] IUnit Unit { get; set; }
    
    IDecisionTreeNode MakeDecision(AiContext context);
    IDecisionTreeNode Clone();
  }
}