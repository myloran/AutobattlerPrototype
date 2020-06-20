using PlasticFloor.EventBus;
using Shared.Abstraction;
using Shared.OkwyLogging;
using Shared.Shared.Client;
using Shared.Shared.Client.Events;
using View.Exts;
using View.NUnit;
using View.NUnit.States;
using View.Presenters;
using Logger = Shared.OkwyLogging.Logger;

namespace Controller.NBattleSimulation {
  public class AttackController : IEventHandler<ApplyDamageEvent>, 
      IEventHandler<DeathEvent>, IEventHandler<StartAttackEvent>, ISimulationTick {
    public AttackController(BoardPresenter boardPresenter, 
        UnitTooltipController unitTooltipController) {
      this.boardPresenter = boardPresenter;
      this.unitTooltipController = unitTooltipController;
    }

    public void HandleEvent(ApplyDamageEvent e) {
      if (!boardPresenter.ContainsUnit(e.Coord)) {
        log.Error("No unit view at coord:");
        return;
      }

      boardPresenter.GetUnit(e.Coord).SetHealth(e.Health.Float);
      unitTooltipController.UpdateHealth(boardPresenter.GetUnit(e.Coord), e.Health.Float);
    }

    public void HandleEvent(DeathEvent e) {
      boardPresenter.GetUnit(e.Coord).gameObject.Hide();
      boardPresenter.RemoveUnit(e.Coord);
    }
    
    public void HandleEvent(StartAttackEvent e) => 
      boardPresenter.GetUnit(e.Coord).ChangeStateTo(EState.Attacking);

    public void SimulationTick(float time) { }

    readonly BoardPresenter boardPresenter;
    readonly UnitTooltipController unitTooltipController;
    static readonly Logger log = MainLog.GetLogger(nameof(AttackController));
  }
}