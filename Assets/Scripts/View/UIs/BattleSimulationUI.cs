using UnityEngine.UI;

namespace View.UIs {
  public class BattleSimulationUI : AutoReferencer<BattleSimulationUI> {
    public Button BExecuteNextDecision,
      BExecuteAllDecisions;
    public Toggle OStartBattle;
    
    public void Disable() => SetEnabled(false);

    public void SetEnabled(bool isOn) {
      BExecuteNextDecision.interactable = isOn;
      BExecuteAllDecisions.interactable = isOn;
    }
  }
}