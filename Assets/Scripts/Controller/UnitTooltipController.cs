using Shared;
using View.Exts;
using View.UI;

namespace Controller {
  public class UnitTooltipController {
    readonly UnitTooltipUI ui;

    public UnitTooltipController(UnitTooltipUI ui) {
      this.ui = ui;
    }

    public void Show(UnitInfo info) {
      ui.SetUnitData(info);
      ui.Show();
    }

    public void Hide() => ui.Hide();
  }
}