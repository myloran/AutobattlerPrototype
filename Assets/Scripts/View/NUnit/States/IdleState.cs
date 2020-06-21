using UnityEngine;

namespace View.NUnit.States {
  public class IdleState : BaseState {
    public IdleState(UnitView unit) : base(unit) { }

    public override void OnEnter() => Unit.Animator.SetBool(isIdle, true);
    public override void OnExit() => Unit.Animator.SetBool(isIdle, false);
    
    static readonly int isIdle = Animator.StringToHash("IsIdle");
  }
}