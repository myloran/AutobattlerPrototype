using Model.NAI;
using Shared;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Model.NUnit.Abstraction {
  public interface IMovement {
    Coord StartingCoord { get; set; }
    Coord TakenCoord { get; set; }
    Coord Coord { get; set; }
    MoveInfo NextMove { get; set; }

    F32 TimeToMove(bool isDiagonal = true);
  }
}