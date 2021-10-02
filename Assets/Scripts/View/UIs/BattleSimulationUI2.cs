using UnityEngine.UIElements;

namespace View.UIs {
  public class BattleSimulationUI2 : IUIReferencer {
    public Button BExecuteNextDecision,
      BExecuteAllDecisions,
      BExecuteInRealtime;
    public Toggle OStart,
      OPause;
    public Slider SSpeed;

    public void Init(UIDocument document) {
      this.FillReferences(document);
      document.rootVisualElement.visible = false;
    }

    // public void Disable() => SetEnabled(false);
    //
    // public void SetEnabled(bool isOn) {
    //   BExecuteNextDecision.interactable = isOn;
    //   BExecuteAllDecisions.interactable = isOn;
    //   BExecuteInRealtime.interactable = isOn;
    // }
  }
}