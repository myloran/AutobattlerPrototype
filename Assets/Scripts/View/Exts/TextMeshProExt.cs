using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace View.Exts {
  public static class TextMeshProExt {
    public static void ResetOptions(this TMP_Dropdown dropdown, IEnumerable<string> names) {
      var options = names.Select(n => new TMP_Dropdown.OptionData(n));
      dropdown.options.Clear();
      dropdown.options.AddRange(options);
    }
  }
}