using static Shared.Const;
using static Shared.Poco.EPlayer;

namespace Shared.Poco {
  public enum EPlayer {
    First = 0,
    Second = 1,
  }

  public static class EPlayerExt {
    public static int BenchId(this EPlayer player) => player == First ? Player1BenchId : Player2BenchId;
    public static EPlayer Opposite(this EPlayer player) => player == First ? Second : First;
    public static EPlayer IsOpposite(this EPlayer player) => player == First ? Second : First;
  }
}