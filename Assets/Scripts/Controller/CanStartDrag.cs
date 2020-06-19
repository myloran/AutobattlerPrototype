using Controller.NBattleSimulation;
using Controller.NDebug;
using Controller.UnitDrag;
using Shared;
using View.UIs;

namespace Controller {
  public class CanStartDrag : IPredicate<DragInfo> {
    public CanStartDrag(BattleStateController battleStateController, 
        UnitModelDebugController unitModelDebugController, BattleSetupUI battleSetupUI, 
        UnitTooltipController unitTooltipController) {
      this.battleStateController = battleStateController;
      this.unitModelDebugController = unitModelDebugController;
      this.battleSetupUI = battleSetupUI;
      this.unitTooltipController = unitTooltipController;
    }
    
    public bool Check(DragInfo e) {
      if (battleStateController.IsBattleStarted) {
        unitModelDebugController.SelectUnitModel(e.StartCoord);
        unitTooltipController.Show(e.Unit);
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