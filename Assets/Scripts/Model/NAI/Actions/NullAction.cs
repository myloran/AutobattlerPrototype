using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NAI.Actions {
  public class NullAction : BaseAction {
    public NullAction(IUnit unit, IEventBus bus) : base(unit, bus) { }
    
    public override IDecisionTreeNode MakeDecision(AiContext context) => this;
  }
}