using UnityEngine;

namespace View.Exts {
  public static class Vector3Ext {
    public static Vector3 WithX(this Vector3 self, float x) => 
      new Vector3(self.x + x, self.y, self.z);
    
    public static Vector3 WithY(this Vector3 self, float y) => 
      new Vector3(self.x, self.y + y, self.z);
  }
}