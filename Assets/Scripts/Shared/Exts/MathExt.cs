namespace Shared.Exts {
  public static class MathExt {
    public static int Clamp(int value, int min, int max) {
      if (value < min)
        value = min;
      else if (value > max)
        value = max;
      return value;
    }
  }
}