using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using View.Exts;

namespace View.UIs {
  public class BattleSetupUI : AutoReferencer<BattleSetupUI> {
    public TMP_Dropdown DUnits,
      DPlayers;

    public Button BAdd,
      BRemove;

    public void SetDropdownOptions(IEnumerable<string> names) => DUnits.ResetOptions(names);

    public string GetSelectedUnitName => DUnits.options[DUnits.value].text;
    public int GetSelectedPlayerId => DPlayers.value;
  }
}