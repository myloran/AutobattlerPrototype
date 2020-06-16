namespace View.NUnit.States {
  public class MoveState : BaseState{
    public MoveState(UnitView unit) : base(unit) { }

    public override void OnEnter() => Unit.Animator.SetBool("IsWalking", true);
    public override void OnExit() => Unit.Animator.SetBool("IsWalking", false);
  }
}