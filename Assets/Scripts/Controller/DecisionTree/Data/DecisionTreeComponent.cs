using Controller.DecisionTree.Visitor;
using MessagePack;
using Model.NAI.NDecisionTree;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Controller.DecisionTree.Data {
  [Union(0, typeof(ActionData))]
  [Union(1, typeof(DecisionData))]
  [MessagePackObject]
  public abstract class DecisionTreeComponent {
    [JsonConverter(typeof(StringEnumConverter))]
    [Key(0)] public EDecisionTreeType Type;

    public abstract T Accept<T>(IDecisionTreeDataVisitor<T> decisionTreeDataVisitor);
  }
}