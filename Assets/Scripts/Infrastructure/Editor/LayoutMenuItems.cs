using UnityEditor;

namespace Editor {
  public class LayoutMenuItems : EditorWindow {
    [MenuItem("Okwy/LayoutShortcuts/1 %#&1 ", false, 999)]
    static void Layout1() {
      EditorApplication.ExecuteMenuItem("Window/Layouts/AutoBattler Default");
    }
 
    [MenuItem("Okwy/LayoutShortcuts/2 %#&2", false, 999)]
    static void Layout2() {
      EditorApplication.ExecuteMenuItem("Window/Layouts/AutoBattler UI");
    }
     
    [MenuItem("Okwy/LayoutShortcuts/3 %#&3", false, 999)]
    static void Layout3() {
      EditorApplication.ExecuteMenuItem("Window/Layouts/AutoBattler UI Debug");
    }
  }
}