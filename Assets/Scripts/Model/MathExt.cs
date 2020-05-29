namespace Model {
  public class MathExt {
    public static float Clamp(float value, float min, float max) {
      if ((double) value < (double) min)
        value = min;
      else if ((double) value > (double) max)
        value = max;
      return value;
    }
  }
}