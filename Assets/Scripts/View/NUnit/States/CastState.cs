using UnityEngine;

namespace View.NUnit.States {
  public class CastState : BaseState {
    public CastState(UnitView unit) : base(unit) { }

    public override void OnEnter() => Unit.Animator.SetBool(isCasting, true);
    public override void OnExit() => Unit.Animator.SetBool(isCasting, false);
    
    static readonly int isCasting = Animator.StringToHash("IsCasting");
  }
}