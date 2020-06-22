using System;
using Controller.Update;
using UnityEngine;
using View.UIs;
using static UnityEngine.Input;
using static UnityEngine.KeyCode;

namespace Controller.NDebug {
  public class UIDebugController : ITick {
    public UIDebugController(BattleSetupUI battleSetupUI, BattleSaveUI battleSaveUI, 
        BattleSimulationUI battleSimulationUI, UnitModelDebugController unitModelDebugController) {
      battleSave = new ComponentEnabler(SetActive(battleSaveUI));
      battleSetup = new ComponentEnabler(SetActive(battleSetupUI));
      battleSimulation = new ComponentEnabler(SetActive(battleSimulationUI));
      unitModelDebug = new ComponentEnabler(unitModelDebugController.SetActive);
    }

    Action<bool> SetActive(Component component) => b => component.gameObject.SetActive(b);

    public void Tick() {
      if (GetKeyDown(F1)) battleSetup.Toggle();
      if (GetKeyDown(F2)) battleSave.Toggle();
      if (GetKeyDown(F3)) battleSimulation.Toggle();
      if (GetKeyDown(F4)) unitModelDebug.Toggle();
    }

    readonly ComponentEnabler battleSetup, 
      battleSave, 
      battleSimulation, 
      unitModelDebug;

    class ComponentEnabler {
      public ComponentEnabler(Action<bool> action, bool isOn = false) {
        this.action = action;
        this.isOn = isOn;
      }

      public void Toggle() {
        isOn = !isOn;
        action(isOn);
      }
      
      readonly Action<bool> action;
      bool isOn;
    }
  }
}