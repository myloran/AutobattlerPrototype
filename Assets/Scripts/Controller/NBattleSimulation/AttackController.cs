using PlasticFloor.EventBus;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using UnityEngine;
using View;
using View.UI;

namespace Controller.NBattleSimulation {
  public class AttackController : IEventHandler<ApplyDamageEvent>, IEventHandler<DeathEvent>,
      ISimulationTick {
    public AttackController(BoardView board, UnitTooltipUI unitTooltipUI) {
      this.board = board;
      this.unitTooltipUI = unitTooltipUI;
    }

    public void HandleEvent(ApplyDamageEvent e) {
      board.Units[e.Coord].Info.Health = e.Health;
      // unitTooltipUI.SetHealth(e.Health);
    }

    public void HandleEvent(DeathEvent e) {
      Object.Destroy(board.Units[e.Coord].gameObject);
      board.Units.Remove(e.Coord);
    }

    public void SimulationTick(float time) {
      
    }

    readonly BoardView board;
    readonly UnitTooltipUI unitTooltipUI;
  }
}