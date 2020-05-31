using PlasticFloor.EventBus;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using View;

namespace Controller.BattleSimulation {
  public class AttackController : IEventHandler<ApplyDamageEvent>, ISimulationTick {
    public AttackController(BoardView board) => this.board = board;

    public void HandleEvent(ApplyDamageEvent e) => 
      board.GetUnit(e.Coord).Info.Health = e.Health;

    public void SimulationTick(float time) {
      
    }

    readonly BoardView board;
  }
}