using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;
using View.NUnit.States;
using View.Presenters;

namespace Controller.NBattleSimulation {
  public class AnimationController : IEventHandler<StartMoveEvent>, IEventHandler<IdleEvent>,
      IEventHandler<StartAttackEvent>, IEventHandler<StartCastEvent>, IEventHandler<UpdateStunDurationEvent>,
      IEventHandler<ContinueMoveEvent> {
    public AnimationController(BoardPresenter board) => this.board = board;

    public void HandleEvent(StartMoveEvent e) => board.TryGetUnit(e.From)?.ChangeStateTo(EState.Walking);
    public void HandleEvent(ContinueMoveEvent e) => board.TryGetUnit(e.Coord)?.ChangeStateTo(EState.Walking);
    public void HandleEvent(IdleEvent e) => board.TryGetUnit(e.Coord)?.ChangeStateTo(EState.Idle);
    public void HandleEvent(UpdateStunDurationEvent e) => board.TryGetUnit(e.Coord)?.ChangeStateTo(EState.Idle);
    public void HandleEvent(StartAttackEvent e) => board.TryGetUnit(e.Coord)?.ChangeStateTo(EState.Attacking);
    public void HandleEvent(StartCastEvent e) => board.TryGetUnit(e.Coord)?.ChangeStateTo(EState.Casting);

    readonly BoardPresenter board;
  }
}