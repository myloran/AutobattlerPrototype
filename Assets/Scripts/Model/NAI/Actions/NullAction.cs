using Model.NAI.NDecisionTree;
using Model.NBattleSimulation;
using Model.NUnit;
using PlasticFloor.EventBus;

namespace Model.NAI.Actions {
  public class NullAction : BaseAction {
    public NullAction(Unit unit, IEventBus bus) : base(unit, bus) { }
    
    public override IDecisionTreeNode MakeDecision(AiContext context) => this;
  }
}