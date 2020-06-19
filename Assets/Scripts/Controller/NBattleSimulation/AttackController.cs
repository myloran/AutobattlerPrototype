using PlasticFloor.EventBus;
using Shared;
using Shared.Abstraction;
using Shared.OkwyLogging;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using UnityEngine;
using View;
using View.Exts;
using View.NUnit;
using View.NUnit.States;
using View.Presenters;
using View.UIs;
using Logger = Shared.OkwyLogging.Logger;

namespace Controller.NBattleSimulation {
  public class AttackController : IEventHandler<ApplyDamageEvent>, 
      IEventHandler<DeathEvent>, IEventHandler<StartAttackEvent>, ISimulationTick {
    public AttackController(BaseBoard<UnitView, PlayerPresenter> board, 
        UnitTooltipController unitTooltipController) {
      this.board = board;
      this.unitTooltipController = unitTooltipController;
    }

    public void HandleEvent(ApplyDamageEvent e) {
      if (!board.ContainsUnitAt(e.Coord)) {
        log.Error("No unit view at coord:");
        return;
      }

      board[e.Coord].SetHealth(e.Health.Float);
      unitTooltipController.UpdateHealth(board[e.Coord], e.Health.Float);
    }

    public void HandleEvent(DeathEvent e) {
      board[e.Coord].gameObject.Hide();
      board.RemoveUnit(e.Coord);
    }
    
    public void HandleEvent(StartAttackEvent e) => 
      board[e.Coord].ChangeStateTo(EState.Attacking);

    public void SimulationTick(float time) { }

    readonly BaseBoard<UnitView, PlayerPresenter> board;
    readonly UnitTooltipController unitTooltipController;
    static readonly Logger log = MainLog.GetLogger(nameof(AttackController));
  }
}