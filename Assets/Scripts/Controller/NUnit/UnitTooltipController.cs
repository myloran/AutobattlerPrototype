using System;
using Controller.Exts;
using UniRx;
using View.Exts;
using View.NUnit;
using View.UIs;

namespace Controller.NUnit {
  public class UnitTooltipController {
    readonly UnitTooltipUI ui;
    readonly UnitSelectionController unitSelectionController;

    public UnitTooltipController(UnitTooltipUI ui, 
        UnitSelectionController unitSelectionController) {
      this.ui = ui;
      this.unitSelectionController = unitSelectionController;
    }

    public void SubToUnitSelection(CompositeDisposable disposable) {
      unitSelectionController.UnitSelected.Subscribe(UpdateTooltip).AddTo(disposable);
      unitSelectionController.UnitDeselected.Subscribe(ui.Hide).AddTo(disposable);
    }  

    void UpdateTooltip(UnitSelectedEvent e) {
      unit = e.Unit;
      ui.SetUnitData(e.Unit.Stats);
      ui.Show();
    }

    public void UpdateHealth(UnitView unit, float health) {
      if (unit == this.unit) 
        ui.SetHealth(health);
    }
    
    UnitView unit;
  }
}