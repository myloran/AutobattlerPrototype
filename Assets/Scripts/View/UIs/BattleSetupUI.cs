using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

namespace View.UIs {
  public class BattleSetupUI : AutoReferencer<BattleSetupUI> {
    public TMP_Dropdown DUnits,
      DPlayers;

    public Button BAdd,
      BRemove;

    public void Init(IEnumerable<string> names) {
      var options = names.Select(n => new TMP_Dropdown.OptionData(n)); //TODO: make extension
      DUnits.options.Clear();
      DUnits.options.AddRange(options);
    }

    public string GetSelectedUnitName => DUnits.options[DUnits.value].text;
    public int GetSelectedPlayerId => DPlayers.value;
  }
}