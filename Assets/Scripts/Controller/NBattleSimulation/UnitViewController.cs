using PlasticFloor.EventBus;
using Shared.Shared.Client.Events;
using View.Presenters;

namespace Controller.NBattleSimulation {
  public class UnitViewController : IEventHandler<UpdateSilenceDurationEvent> {
    public UnitViewController(BoardPresenter board) => this.board = board;
    
    public void HandleEvent(UpdateSilenceDurationEvent e) => 
      board.GetUnit(e.Coord).UpdateSilenceDuration(e.Duration.Float);

    readonly BoardPresenter board;
  }
}