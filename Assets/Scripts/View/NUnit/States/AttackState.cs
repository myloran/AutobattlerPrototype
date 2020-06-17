namespace View.NUnit.States {
  public class AttackState : BaseState {
    public AttackState(UnitView unit) : base(unit) { }

    public override void OnEnter() => Unit.Animator.SetBool("IsAttacking", true);
    public override void OnExit() => Unit.Animator.SetBool("IsAttacking", false);
  }
}