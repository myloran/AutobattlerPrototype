using FixMath;
using Shared;

namespace Model.NUnit.Abstraction {
  public interface IMovement {
    Coord StartingCoord { get; set; }
    Coord TakenCoord { get; set; }
    Coord Coord { get; set; }

    F32 TimeToMove(bool isDiagonal = true);
  }
}