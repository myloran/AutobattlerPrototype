using UnityEngine.UI;
using View.Exts;

namespace View {
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