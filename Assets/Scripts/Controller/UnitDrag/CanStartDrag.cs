using Controller.NBattleSimulation;
using Controller.NDebug;
using Controller.NUnit;
using Shared;
using Shared.Abstraction;
using Shared.Primitives;
using View.UIs;

namespace Controller.UnitDrag {
  public class CanStartDrag : IPredicate<UnitSelectedEvent> {
    public CanStartDrag(BattleStateController battleStateController, BattleSetupUI battleSetupUI) {
      this.battleStateController = battleStateController;
      this.battleSetupUI = battleSetupUI;
    }
    
    public bool Check(UnitSelectedEvent e) {
      if (battleStateController.IsBattleStarted) return false;
      if (e.Unit.Player != (EPlayer) battleSetupUI.GetSelectedPlayerId) return false;

      return true;
    }

    readonly BattleStateController battleStateController;
    readonly UnitModelDebugController unitModelDebugController;
    readonly BattleSetupUI battleSetupUI;
  }
}