using Controller.NUnit;
using PlasticFloor.EventBus;
using Shared.Addons.OkwyLogging;
using Shared.Shared.Client.Events;
using UnityEngine;
using View.Exts;
using View.Presenters;
using Logger = Shared.Addons.OkwyLogging.Logger;

namespace Controller.NBattleSimulation {
  public class AttackController : IEventHandler<UpdateHealthEvent>, 
      IEventHandler<DeathEvent>, IEventHandler<UpdateManaEvent> {
    public AttackController(BoardPresenter boardPresenter, 
        UnitTooltipController unitTooltipController) {
      this.boardPresenter = boardPresenter;
      this.unitTooltipController = unitTooltipController;
    }

    public void HandleEvent(UpdateHealthEvent e) {
      var unit = boardPresenter.TryGetUnit(e.Coord);
      if (unit == null) return;
      
      unit.SetHealth(e.Health.Float);
      unitTooltipController.UpdateHealth(unit, e.Health.Float);
    }
    
    public void HandleEvent(UpdateManaEvent e) {
      var unit = boardPresenter.TryGetUnit(e.Coord);
      if (unit == null) return;
      
      unit.SetMana(e.Mana.Float);
    }

    public void HandleEvent(DeathEvent e) {
      boardPresenter.TryGetUnit(e.Coord).Hide();
      boardPresenter.RemoveUnit(e.Coord);
    }
    
    readonly BoardPresenter boardPresenter;
    readonly UnitTooltipController unitTooltipController;
  }
}