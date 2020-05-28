using UnityEngine;

namespace View.Exts {
  public static class ComponentExt {
    public static void Show(this Component component) => component.gameObject.SetActive(true);
  }
}