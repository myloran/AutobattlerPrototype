using Shared.Addons.Examples.FixMath;
using Shared.Poco;

namespace Model.NAI {
  public class MoveInfo {
    public Coord Coord;
    public F32 Time;

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