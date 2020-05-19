using UnityEngine;

namespace View {
  public static class Vector3Ext {
    public static Vector3 WithY(this Vector3 self, float y) => 
      new Vector3(self.x, self.y + y, self.z);
  }
}