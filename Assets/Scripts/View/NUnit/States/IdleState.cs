namespace View.NUnit.States {
  public class IdleState : BaseState {
    public IdleState(UnitView unit) : base(unit) { }

    public override void OnEnter() => Unit.Animator.SetBool("IsIdle", true);
    public override void OnExit() => Unit.Animator.SetBool("IsIdle", false);
  }
}