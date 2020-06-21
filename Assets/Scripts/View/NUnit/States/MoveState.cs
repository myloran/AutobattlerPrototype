using UnityEngine;

namespace View.NUnit.States {
  public class MoveState : BaseState{
    public MoveState(UnitView unit) : base(unit) { }

    public override void OnEnter() => Unit.Animator.SetBool(isWalking, true);
    public override void OnExit() => Unit.Animator.SetBool(isWalking, false);
    
    static readonly int isWalking = Animator.StringToHash("IsWalking");
  }
}