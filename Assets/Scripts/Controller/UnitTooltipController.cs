using Shared;
using View.Exts;
using View.NUnit;
using View.UIs;

namespace Controller {
  public class UnitTooltipController {
    readonly UnitTooltipUI ui;

    public UnitTooltipController(UnitTooltipUI ui) {
      this.ui = ui;
    }

    public void Show(UnitView unit) {
      this.unit = unit;
      ui.SetUnitData(unit.Info);
      ui.Show();
    }

    public void Hide() => ui.Hide();
    

    public void UpdateHealth(UnitView unit, float health) {
      if (unit == this.unit) 
        ui.SetHealth(health);
    }
    
    UnitView unit;
  }
}