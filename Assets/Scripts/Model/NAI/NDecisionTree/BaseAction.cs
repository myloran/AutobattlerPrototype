using Model.NAI.Commands;
using Model.NBattleSimulation;
using Model.NUnit;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseAction : IDecisionTreeNode {
    public IUnit Unit;
    protected IEventBus Bus;
    
    public virtual EDecision Type { get; } = EDecision.BaseAction;
    
    protected void Init(IUnit unit, IEventBus bus) {
      Bus = bus;
      Unit = unit;
    }

    public abstract IDecisionTreeNode MakeDecision(AiContext context);
  }
}