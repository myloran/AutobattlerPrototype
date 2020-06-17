using PlasticFloor.EventBus;
using Shared;
using Shared.Abstraction;
using Shared.OkwyLogging;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using UnityEngine;
using View;
using View.NUnit;
using View.NUnit.States;
using View.Presenters;
using View.UIs;
using Logger = Shared.OkwyLogging.Logger;

namespace Controller.NBattleSimulation {
  public class AttackController : IEventHandler<ApplyDamageEvent>, 
      IEventHandler<DeathEvent>, IEventHandler<StartAttackEvent>, ISimulationTick {
    public AttackController(BaseBoard<UnitView, PlayerPresenter> board, 
        UnitTooltipUI unitTooltipUI) {
      this.board = board;
      this.unitTooltipUI = unitTooltipUI;
    }

    public void HandleEvent(ApplyDamageEvent e) {
      if (!board.ContainsUnitAt(e.Coord)) {
        log.Error("No unit view at coord:");
        return;
      }

      board[e.Coord].SetHealth(e.Health.Float);
      // unitTooltipUI.SetHealth(e.Health);
    }

    public void HandleEvent(DeathEvent e) {
      Object.Destroy(board[e.Coord].gameObject); //TODO: hide instead of destroy
      board.RemoveUnit(e.Coord);
    }
    
    public void HandleEvent(StartAttackEvent e) => 
      board[e.Coord].ChangeStateTo(EState.Attacking);

    public void SimulationTick(float time) { }

    readonly BaseBoard<UnitView, PlayerPresenter> board;
    readonly UnitTooltipUI unitTooltipUI;
    static readonly Logger log = MainLog.GetLogger(nameof(AttackController));
  }
}