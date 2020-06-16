namespace View.NUnit.States {
  public abstract class BaseState : IState {
    protected UnitView Unit;

    protected BaseState(UnitView unit) => Unit = unit;
    
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
  }
}