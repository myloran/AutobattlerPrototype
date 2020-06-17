using Model.NBattleSimulation;
using Model.NUnit;
using PlasticFloor.EventBus;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseAction : IDecisionTreeNode {
    public virtual EDecision Type { get; } = EDecision.BaseAction;
    public readonly Unit Unit;
    protected readonly IEventBus Bus;
    
    protected BaseAction(Unit unit, IEventBus bus) {
      Bus = bus;
      Unit = unit;
    }

    public abstract IDecisionTreeNode MakeDecision(AiContext context);
  }
}