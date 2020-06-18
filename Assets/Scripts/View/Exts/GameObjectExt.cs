using UnityEngine;

namespace View.Exts {
  public static class GameObjectExt {
    public static void Show(this GameObject obj) => obj.SetActive(true);
    public static void Hide(this GameObject obj) => obj.SetActive(false);
  }
}