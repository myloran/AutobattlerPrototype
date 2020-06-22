using System.Collections.Generic;
using System.Linq;
using Controller.Exts;
using TMPro;
using UniRx;
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
      Reset(names);
      Subs();
    }

    void Subs() {
      FSaveName.onValueChanged.AsObservable().Subscribe(CheckSaveEnabled).AddTo(this);
      BAdd.Sub(CheckLoadEnabled);
    }

    void Reset(IEnumerable<string> names) {
      DSaves.ResetOptions(names);

      if (!DSaves.options.Any()) BLoad.Disable();
      BLoadPrevious.Disable();
      BAdd.Disable();
    }

    public string SaveName => FSaveName.text;
    public string GetSelectedSaveName => DSaves.options[DSaves.value].text;

    void CheckSaveEnabled(string text) => BAdd.interactable = text.Length > 0;
    
    void CheckLoadEnabled() {
      BLoad.Enable();
      BLoadPrevious.Enable();
    }
  }
}