using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using PlasticFloor.EventBus;

namespace Model.NAI.NDecisionTree {
  public abstract class BaseAction : IDecisionTreeNode {
    public IUnit Unit { get; set; }
    protected IEventBus Bus;
    
    public virtual EDecision Type { get; } = EDecision.BaseAction;

    public void Init(IEventBus bus) => Bus = bus;

    protected IDecisionTreeNode BaseClone(BaseAction from, BaseAction to) {
      to.Bus = from.Bus;
      return to;
    }

    public abstract IDecisionTreeNode Clone();
    public abstract IDecisionTreeNode MakeDecision(AiContext context);
  }
}