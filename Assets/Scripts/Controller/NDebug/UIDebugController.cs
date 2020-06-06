using UnityEngine;
using View;
using View.UI;

namespace Controller.NDebug {
  public class UIDebugController : ITick {
    public UIDebugController(BattleSetupUI battleSetupUI, BattleSaveUI battleSaveUI, 
        BattleSimulationUI battleSimulationUI, UnitModelDebugController unitModelDebugController) {
      this.battleSetupUI = battleSetupUI;
      this.battleSaveUI = battleSaveUI;
      this.battleSimulationUI = battleSimulationUI;
      this.unitModelDebugController = unitModelDebugController;
    }

    public void Tick() {
      if (Input.GetKeyDown(KeyCode.F1)) {
        isBattleSetupUIOn = !isBattleSetupUIOn;
        battleSetupUI.gameObject.SetActive(isBattleSetupUIOn);
      }
      if (Input.GetKeyDown(KeyCode.F2)) {
        isBattleSaveUIOn = !isBattleSaveUIOn;
        battleSaveUI.gameObject.SetActive(isBattleSaveUIOn);
      }
      if (Input.GetKeyDown(KeyCode.F3)) {
        isBattleSimulationUIOn = !isBattleSimulationUIOn;
        battleSimulationUI.gameObject.SetActive(isBattleSimulationUIOn);
      }
      if (Input.GetKeyDown(KeyCode.F4)) {
        isUnitModelDebugControllerOn = !isUnitModelDebugControllerOn;
        unitModelDebugController.SetActive(isUnitModelDebugControllerOn);
      }
    }

    readonly BattleSetupUI battleSetupUI;
    readonly BattleSaveUI battleSaveUI;
    readonly BattleSimulationUI battleSimulationUI;
    readonly UnitModelDebugController unitModelDebugController;
    
    bool isBattleSetupUIOn,
      isBattleSaveUIOn,
      isBattleSimulationUIOn,
      isUnitModelDebugControllerOn;
  }
}