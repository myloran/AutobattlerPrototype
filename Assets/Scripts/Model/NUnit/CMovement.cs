using FixMath;
using Model.NUnit.Abstraction;
using Shared;
using static FixMath.F32;

namespace Model.NUnit {
  public class CMovement : IMovement {
    public Coord StartingCoord { get; set; }
    public Coord TakenCoord { get; set; } = Coord.Invalid;
    public Coord Coord { get; set; }
    
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

    public override string ToString() => $"{nameof(StartingCoord)}: {StartingCoord}, {nameof(TakenCoord)}: {TakenCoord}, {nameof(Coord)}: {Coord}, {nameof(speed)}: {speed}";
    
    static F32 Straight => One;
    static F32 Diagonal => Sqrt(Two);
    readonly F32 speed;
  }
}