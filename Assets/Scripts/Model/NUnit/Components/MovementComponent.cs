using System;
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
    public bool IsMovePaused { get; private set; }
    public F32 MovementTimeLeft { get; private set; } = MaxBattleDuration;
    
    public Action OnMoveFinished { get; set; } = () => { };
    
    public MovementComponent(Coord coord, F32 speed) {
      StartingCoord = coord;
      this.speed = speed;
    }

    public bool CanStartMovement(F32 currentTime) => moveEndTime < currentTime;

    public void StartMovement(F32 endTime) {
      moveEndTime = endTime;
      TakenCoord = NextMove.Coord;
      IsMovePaused = false;
    }

    public void FinishMovement() {
      Coord = TakenCoord;
      TakenCoord = Coord.Invalid;
      moveEndTime = -MaxBattleDuration;
      OnMoveFinished();
    }

    public void TryPauseMovement(F32 currentTime) {
      IsMovePaused = TakenCoord != Coord.Invalid;
      MovementTimeLeft = moveEndTime - currentTime;
    }

    public F32 TimeToMove(bool isDiagonal) => isDiagonal 
      ? diagonal * speed : straight * speed;
    
    public void Reset() {
      Coord = StartingCoord;
      TakenCoord = Coord.Invalid;
      NextMove = new MoveInfo();
      moveEndTime = -MaxBattleDuration;
      IsMovePaused = false;
      MovementTimeLeft = MaxBattleDuration;
      OnMoveFinished = () => { };
    }

    public override string ToString() => $"{nameof(Coord)}: {Coord}, {nameof(TakenCoord)}: {TakenCoord}, {nameof(StartingCoord)}: {StartingCoord}, {nameof(speed)}: {speed}, {nameof(moveEndTime)}: {moveEndTime}, {nameof(NextMove)}: {NextMove}, {nameof(IsMovePaused)}: {IsMovePaused}, {nameof(MovementTimeLeft)}: {MovementTimeLeft}";

    readonly F32 straight = One;
    readonly F32 diagonal = Sqrt(Two);
    readonly F32 speed;
    F32 moveEndTime;
  }
}