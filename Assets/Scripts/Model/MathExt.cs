using System;

namespace Model {
  public static class MathExt {
    public static bool IsEqualTo(this float self, float other) => 
      Math.Abs(self - other) < float.Epsilon;
    
    static float floatError => 0.00001f;

    public static float Clamp(float value, float min, float max) {
      if ((double) value < (double) min)
        value = min;
      else if ((double) value > (double) max)
        value = max;
      return value;
    }
  }
}