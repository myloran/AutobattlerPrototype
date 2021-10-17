using UnityEditor;

namespace Editor {
  public class OkwyUtils : EditorWindow {
    [MenuItem("Okwy/Set Dirty")]
    static void SetObjectsDirty() {
      foreach (var o in Selection.objects) 
        EditorUtility.SetDirty(o);
    }
  }
}