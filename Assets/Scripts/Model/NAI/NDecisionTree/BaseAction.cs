using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseAction : IDecisionTreeNode {
    public IUnit Unit { get; set; }
    protected IEventBus Bus;
    
    public virtual EDecision Type { get; } = EDecision.BaseAction;

    public void Init(IUnit unit, IEventBus bus) {
      Bus = bus;
      Unit = unit;
    }
    
    protected IDecisionTreeNode BaseClone(BaseAction from, BaseAction to) {
      to.Bus = from.Bus;
      return to;
    }

    public abstract IDecisionTreeNode Clone();
    public void SetUnit(IUnit unit) => Unit = unit;

    public abstract IDecisionTreeNode MakeDecision(AiContext context);
  }
}