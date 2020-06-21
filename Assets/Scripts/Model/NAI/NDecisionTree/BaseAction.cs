using Model.NAI.Commands;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseAction : IDecisionTreeNode {
    public virtual EDecision Type { get; } = EDecision.BaseAction;
    public readonly IUnit Unit;
    protected readonly IEventBus Bus;
    
    protected BaseAction(IUnit unit, IEventBus bus) {
      Bus = bus;
      Unit = unit;
    }

    public abstract IDecisionTreeNode MakeDecision(AiContext context);
  }
}