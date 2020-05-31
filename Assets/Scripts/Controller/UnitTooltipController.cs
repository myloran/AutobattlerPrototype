using Controller.BattleSimulation;
using View.Exts;
using View.UI;

namespace Controller {
  public class UnitTooltipController {
    readonly BattleSimulationController controller;
    readonly UnitTooltipUI ui;

    public UnitTooltipController(BattleSimulationController controller, UnitTooltipUI ui) {
      this.controller = controller;
      this.ui = ui;
    }

    public bool IsBattleStarted => controller.IsBattleStarted;

    public void Show() => ui.Show();
    public void Hide() => ui.Hide();
  }
}