using System;

namespace View.NUnit.States {
  public class UnitFsm {
    public UnitFsm(UnitView unit) {
      idle = new IdleState(unit);
      move = new MoveState(unit);
      current = idle;
    }
    
    public void ChangeStateTo(EState state) {
      switch (state) {
        case EState.Idle:
          ChangeStateTo(idle);
          break;
        case EState.Walking:
          ChangeStateTo(move);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(state), state, null);
      }
    }
    
    void ChangeStateTo(IState state) {
      current.OnExit();
      current = state;
      current.OnEnter();
    }

    readonly IState idle,move;
    IState current;
  }
}