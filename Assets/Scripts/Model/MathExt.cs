using System;

namespace Model {
  public static class MathExt {
    public static bool IsEqualTo(this float self, float other) => 
      Math.Abs(self - other) < float.Epsilon;

    public static float Clamp(float value, float min, float max) {
      if ((double) value < (double) min)
        value = min;
      else if ((double) value > (double) max)
        value = max;
      return value;
    }
  }
}