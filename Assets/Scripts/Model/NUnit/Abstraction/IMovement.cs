using Shared;
using Shared.Addons.Examples.FixMath;
using Shared.Poco;

namespace Model.NUnit.Abstraction {
  public interface IMovement {
    Coord StartingCoord { get; set; }
    Coord TakenCoord { get; set; }
    Coord Coord { get; set; }

    F32 TimeToMove(bool isDiagonal = true);
  }
}