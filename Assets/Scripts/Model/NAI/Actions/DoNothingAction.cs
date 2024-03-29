using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NAI.Actions {
  public class DoNothing : BaseAction {
    public override EDecision Type { get; } = EDecision.DoNothing;
    public override IDecisionTreeNode Clone() => BaseClone(this, new DoNothing());
    
    public override IDecisionTreeNode MakeDecision(AiContext context) => this;
  }
}