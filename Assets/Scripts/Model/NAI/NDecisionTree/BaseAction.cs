using Model.NBattleSimulation;
using Model.NUnit;
using PlasticFloor.EventBus;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseAction : IDecisionTreeNode {
    protected readonly Unit Unit;
    protected readonly IEventBus Bus;
    
    protected BaseAction(Unit unit, IEventBus bus) {
      this.Bus = bus;
      this.Unit = unit;
    }
    
    public abstract IDecisionTreeNode MakeDecision(AiContext context);
  }
}