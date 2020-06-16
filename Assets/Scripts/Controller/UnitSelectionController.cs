using Controller.NBattleSimulation;
using Controller.NDebug;
using Controller.UnitDrag;
using Shared;
using View.UIs;

namespace Controller {
  public class UnitSelectionController : IPredicate<DragInfo> {
    public UnitSelectionController(BattleStateController battleStateController, UnitModelDebugController unitModelDebugController, BattleSetupUI battleSetupUI) {
      this.battleStateController = battleStateController;
      this.unitModelDebugController = unitModelDebugController;
      this.battleSetupUI = battleSetupUI;
    }
    
    public bool Check(DragInfo e) {
      if (battleStateController.IsBattleStarted) {
        unitModelDebugController.SelectUnitModel(e.StartCoord);
        // unitTooltipController.Show(unit.Info);
        return false;
      }

      if (e.Unit.Player != (EPlayer) battleSetupUI.GetSelectedPlayerId)
        return false;

      return true;
    }
    
    readonly UnitTooltipController unitTooltipController;
    readonly BattleStateController battleStateController;
    readonly UnitModelDebugController unitModelDebugController;
    readonly BattleSetupUI battleSetupUI;
  }
}