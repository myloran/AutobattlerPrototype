using UnityEngine.UI;

namespace View.Exts {
  public static class SelectableExt {
    public static void Enable(this Selectable selectable) => selectable.interactable = true;
    public static void Disable(this Selectable selectable) => selectable.interactable = false;
  }
}