using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using View.Exts;

namespace View.UIs {
  public class BattleSaveUI : AutoReferencer<BattleSaveUI> {
    public TMP_InputField FSaveName;
    public TMP_Dropdown DSaves;

    public Button BAdd,
      BRemove,
      BLoad,
      BLoadPrevious;
    
    public void Init(IEnumerable<string> names) {
      var options = names.Select(n => new TMP_Dropdown.OptionData(n));
      DSaves.options.Clear();
      DSaves.options.AddRange(options);

      if (!DSaves.options.Any()) {
        BLoad.Disable();
      }
      BLoadPrevious.Disable();
      BAdd.Disable();
      FSaveName.onValueChanged.AddListener(CheckSaveEnabled);
      BAdd.onClick.AddListener(CheckLoadEnabled);
    }

    public string SaveName => FSaveName.text;
    public string GetSelectedSaveName => DSaves.options[DSaves.value].text;

    void CheckSaveEnabled(string text) => BAdd.interactable = text.Length > 0;
    
    void CheckLoadEnabled() {
      BLoad.Enable();
      BLoadPrevious.Enable();
    }

    void OnDestroy() {
      FSaveName.onValueChanged.RemoveListener(CheckSaveEnabled);
      BAdd.onClick.RemoveListener(CheckLoadEnabled);
    }
  }
}