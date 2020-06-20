using System;
using View.NUnit.States;

namespace View.NUnit {
  public class UnitFsm {
    public UnitFsm(UnitView unit) {
      idle = new IdleState(unit);
      move = new MoveState(unit);
      attack = new AttackState(unit);
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
        case EState.Attacking:
          ChangeStateTo(attack);
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

    readonly IState idle, 
      move, 
      attack;
    IState current;
  }
}