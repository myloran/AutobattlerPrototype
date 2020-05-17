using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

namespace View.UI {
  public class BattleSetupUI : AutoReferencer<BattleSetupUI> {
    public TMP_Dropdown DUnit,
      DPlayer;

    public Button BAdd,
      BRemove,
      BStartBattle;

    public void Init(List<string> names) {
      var options = names.Select(n => new TMP_Dropdown.OptionData(n));
      DUnit.options.Clear();
      DUnit.options.AddRange(options);
    }

    public string GetSelectedUnitName => DUnit.options[DUnit.value].text;
    public int GetSelectedPlayerId => DPlayer.value;
  }
}