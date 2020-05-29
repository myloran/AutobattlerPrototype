using PlasticFloor.EventBus;
using Shared.Events;
using View;

namespace Controller.BattleSimulation {
  public class MovementController : IEventHandler<StartMoveEvent>, 
      IEventHandler<EndMoveEvent>, ITick {
    public MovementController(BoardView board) {
      this.board = board;
    }
    
    public void HandleEvent(StartMoveEvent e) {
      board.Move(e.From, e.To, e.Duration);
    }

    public void HandleEvent(EndMoveEvent e) {
    }

    public void Update(float time) {
    }

    readonly BoardView board;
  }
}