using Controller.NUnit;
using PlasticFloor.EventBus;
using Shared.Addons.OkwyLogging;
using Shared.Shared.Client.Events;
using View.Exts;
using View.Presenters;
using Logger = Shared.Addons.OkwyLogging.Logger;

namespace Controller.NBattleSimulation {
  public class AttackController : IEventHandler<UpdateHealthEvent>, 
      IEventHandler<DeathEvent> {
    public AttackController(BoardPresenter boardPresenter, 
        UnitTooltipController unitTooltipController) {
      this.boardPresenter = boardPresenter;
      this.unitTooltipController = unitTooltipController;
    }

    public void HandleEvent(UpdateHealthEvent e) { 
      boardPresenter.GetUnit(e.Coord).SetHealth(e.Health.Float);
      unitTooltipController.UpdateHealth(boardPresenter.GetUnit(e.Coord), e.Health.Float);
    }

    public void HandleEvent(DeathEvent e) {
      boardPresenter.GetUnit(e.Coord).Hide();
      boardPresenter.RemoveUnit(e.Coord);
    }
    
    readonly BoardPresenter boardPresenter;
    readonly UnitTooltipController unitTooltipController;
  }
}