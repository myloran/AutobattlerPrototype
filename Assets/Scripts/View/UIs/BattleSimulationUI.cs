using UnityEngine.UI;

namespace View.UIs {
  public class BattleSimulationUI : AutoReferencer<BattleSimulationUI> {
    public Button BExecuteNextDecision,
      BExecuteAllDecisions,
      BExecuteInRealtime;
    public Toggle OStart,
      OPause;
    public Slider SSpeed;

    public void Disable() => SetEnabled(false);

    public void SetEnabled(bool isOn) {
      BExecuteNextDecision.interactable = isOn;
      BExecuteAllDecisions.interactable = isOn;
    }
  }
}