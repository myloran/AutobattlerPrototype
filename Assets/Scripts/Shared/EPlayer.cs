using static Shared.EPlayer;

namespace Shared {
  public enum EPlayer {
    First = 0,
    Second = 1,
  }

  public static class EPlayerExt {
    public static int Y(this EPlayer player) => player == First ? -1 : -2;
    public static EPlayer Opposite(this EPlayer player) => player == First ? Second : First;
  }
}