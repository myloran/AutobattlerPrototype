using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using View.Exts;

namespace View.UIs {
  public class BattleSimulationUI : AutoReferencer<BattleSimulationUI> {
    public Button BExecuteNextDecision,
      BExecuteAllDecisions,
      BExecuteInRealtime;
    public Toggle OStart,
      OPause,
      OTestMode;
    public Slider SSpeed;
    public TMP_Dropdown DBattleTest;
    
    public void SetBattleTests(IEnumerable<string> names) => DBattleTest.ResetOptions(names);
    public string GetSelectedBattleTest => DBattleTest.options[DBattleTest.value].text;
    public void Disable() => SetEnabled(false);

    public void SetEnabled(bool isOn) {
      BExecuteNextDecision.interactable = isOn;
      BExecuteAllDecisions.interactable = isOn;
      BExecuteInRealtime.interactable = isOn;
    }
  }
}