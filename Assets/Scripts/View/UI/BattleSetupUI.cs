using System.Collections.Generic;
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
      
    }
  }
}