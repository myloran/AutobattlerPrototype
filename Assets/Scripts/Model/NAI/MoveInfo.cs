using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Model.NAI {
  public struct MoveInfo {
    public Coord Coord;
    public F32 Time;

    public MoveInfo(Coord coord, F32 time) {
      Coord = coord;
      Time = time;
    }

    public void Update(Coord coord, F32 time) {
      Coord = coord;
      Time = time;
    }

    public void Deconstruct(out Coord coord, out F32 time) {
      coord = Coord;
      time = Time;
    }
  }
}