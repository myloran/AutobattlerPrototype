using UnityEngine;
using View;
using View.UI;

namespace Controller {
  public class DebugUIController : MonoBehaviour {
    public void Init(BattleSetupUI battleSetupUI, BattleSaveUI battleSaveUI, 
        BattleSimulationUI battleSimulationUI) {
      this.battleSetupUI = battleSetupUI;
      this.battleSaveUI = battleSaveUI;
      this.battleSimulationUI = battleSimulationUI;
    }
    
    public void Update() {
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
    }

    BattleSetupUI battleSetupUI;
    BattleSaveUI battleSaveUI;
    BattleSimulationUI battleSimulationUI;
    bool isBattleSetupUIOn,
      isBattleSaveUIOn,
      isBattleSimulationUIOn;
  }
}