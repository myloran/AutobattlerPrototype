using System;
using Controller.Update;
using UnityEngine;
using UnityEngine.UIElements;
using View.UIs;
using View.UIToolkit;
using static UnityEngine.Input;
using static UnityEngine.KeyCode;

namespace Controller.NDebug {
  public class UIDebugController : ITick {
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
    
    public UIDebugController(BattleSetupUI battleSetupUI, BattleSaveUI battleSaveUI,
        BattleSimulationUI battleSimulationUI, UnitModelDebugController unitModelDebugController,
        CommandDebugWindowUI commandDebugWindowUI) {
      commandsDebug = new ComponentEnabler(SetActiveUI(commandDebugWindowUI.Document));
      battleSave = new ComponentEnabler(SetActiveMonoBehaviour(battleSaveUI));
      battleSetup = new ComponentEnabler(SetActiveMonoBehaviour(battleSetupUI));
      battleSimulation = new ComponentEnabler(SetActiveMonoBehaviour(battleSimulationUI));
      unitModelDebug = new ComponentEnabler(unitModelDebugController.SetActive);
    }

    Action<bool> SetActiveMonoBehaviour(Component component) => b => component.gameObject.SetActive(b);
    Action<bool> SetActiveUI(UIDocument document) => b => document.rootVisualElement.visible = b;

    public void Tick() {
      if (GetKeyDown(F1)) battleSetup.Toggle();
      if (GetKeyDown(F2)) battleSave.Toggle();
      if (GetKeyDown(F3)) battleSimulation.Toggle();
      if (GetKeyDown(F4)) unitModelDebug.Toggle();
      if (GetKeyDown(F5)) commandsDebug.Toggle();
    }

    readonly ComponentEnabler battleSetup, 
      battleSave, 
      battleSimulation, 
      unitModelDebug,
      commandsDebug;
  }
}