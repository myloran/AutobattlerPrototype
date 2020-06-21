using UnityEngine;

namespace View.NUnit.States {
  public class AttackState : BaseState {
    public AttackState(UnitView unit) : base(unit) { }

    public override void OnEnter() => Unit.Animator.SetBool(isAttacking, true);
    public override void OnExit() => Unit.Animator.SetBool(isAttacking, false);
    
    static readonly int isAttacking = Animator.StringToHash("IsAttacking");
  }
}