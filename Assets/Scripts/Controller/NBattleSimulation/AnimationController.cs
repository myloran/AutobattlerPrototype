using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;
using View.NUnit.States;
using View.Presenters;

namespace Controller.NBattleSimulation {
  public class AnimationController : IEventHandler<StartMoveEvent>, IEventHandler<IdleEvent>,
      IEventHandler<StartAttackEvent> {
    public AnimationController(BoardPresenter board) => this.board = board;

    public void HandleEvent(StartMoveEvent e) => 
      board.GetUnit(e.From).ChangeStateTo(EState.Walking);

    public void HandleEvent(IdleEvent e) =>
      board.GetUnit(e.Coord).ChangeStateTo(EState.Idle);

    public void HandleEvent(StartAttackEvent e) => 
      board.GetUnit(e.Coord).ChangeStateTo(EState.Attacking);

    readonly BoardPresenter board;
  }
}