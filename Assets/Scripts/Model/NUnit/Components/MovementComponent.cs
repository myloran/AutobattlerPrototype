using Model.NAI;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;
using static Shared.Const;

namespace Model.NUnit.Components {
  public class MovementComponent : IMovement {
    public Coord StartingCoord { get; set; }
    public Coord TakenCoord { get; set; } = Coord.Invalid; //make private
    public Coord Coord { get; set; }
    public MoveInfo NextMove { get; set; }
    public bool WasMovePaused { get; private set; }
    public F32 MovementTimeLeft { get; private set; } = MaxBattleDuration; 
    
    public MovementComponent(Coord coord, F32 speed) {
      StartingCoord = coord;
      this.speed = speed;
    }

    public bool CanStartMovement(F32 currentTime) => moveEndTime < currentTime;

    public void StartMovement(F32 endTime) {
      moveEndTime = endTime;
      TakenCoord = NextMove.Coord;
      WasMovePaused = false;
    }

    public void FinishMovement() {
      Coord = TakenCoord;
      TakenCoord = Coord.Invalid;
      moveEndTime = -MaxBattleDuration;
    }

    public void TryPauseMovement(F32 currentTime) {
      WasMovePaused = TakenCoord != Coord.Invalid;
      MovementTimeLeft = moveEndTime - currentTime;
    }

    public F32 TimeToMove(bool isDiagonal) => isDiagonal 
      ? Diagonal * speed : Straight * speed;
    
    public void Reset() {
      Coord = StartingCoord;
      TakenCoord = Coord.Invalid;
      NextMove = new MoveInfo();
      moveEndTime = -MaxBattleDuration;
      WasMovePaused = false;
      MovementTimeLeft = MaxBattleDuration;
    }

    public override string ToString() => $"{nameof(StartingCoord)}: {StartingCoord}, {nameof(TakenCoord)}: {TakenCoord}, {nameof(Coord)}: {Coord}, {nameof(speed)}: {speed}, {nameof(moveEndTime)}: {moveEndTime}, {nameof(NextMove)}: {NextMove}, {nameof(WasMovePaused)}: {WasMovePaused}, {nameof(MovementTimeLeft)}: {MovementTimeLeft}";
    
    static F32 Straight => One;
    static F32 Diagonal => Sqrt(Two);
    readonly F32 speed;
    F32 moveEndTime;
  }
}