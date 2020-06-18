using System;
using FixMath;
using Shared;
using static FixMath.F32;

namespace Model.NUnit {
  public class CMovement {
    public static F32 Straight => One;
    public static F32 Diagonal => Sqrt(Two);
    public Coord StartingCoord;
    public Coord TakenCoord = Coord.Invalid;
    public Coord Coord;
    
    public CMovement(Coord coord, F32 speed) {
      StartingCoord = coord;
      this.speed = speed;
    }

    public void Reset() {
      Coord = StartingCoord;
      TakenCoord = Coord.Invalid;
    }
    
    public F32 TimeToMove(bool isDiagonal = true) => isDiagonal 
      ? Diagonal * speed : Straight * speed;

    readonly F32 speed;

    public override string ToString() => $"{nameof(StartingCoord)}: {StartingCoord}, {nameof(TakenCoord)}: {TakenCoord}, {nameof(Coord)}: {Coord}, {nameof(speed)}: {speed}";
  }
}