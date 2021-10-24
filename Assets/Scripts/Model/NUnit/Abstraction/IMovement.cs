using Model.NAI;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Model.NUnit.Abstraction {
  public interface IMovement {
    Coord StartingCoord { get; set; }
    Coord TakenCoord { get; set; }
    Coord Coord { get; set; }
    MoveInfo NextMove { get; set; }
    F32 MovementTimeLeft { get; }
    bool IsMovePaused { get; }

    bool CanStartMovement(F32 currentTime);
    void StartMovement(F32 endTime);
    void FinishMovement();
    F32 TimeToMove(bool isDiagonal = true);
  }
}