using Model.NAI;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NUnit.Components {
  public class MovementComponent : IMovement {
    public Coord StartingCoord { get; set; }
    public Coord TakenCoord { get; set; } = Coord.Invalid;
    public Coord Coord { get; set; }
    public MoveInfo NextMove { get; set; }
    
    public MovementComponent(Coord coord, F32 speed) {
      StartingCoord = coord;
      this.speed = speed;
    }

    public void Reset() {
      Coord = StartingCoord;
      TakenCoord = Coord.Invalid;
    }
    
    public F32 TimeToMove(bool isDiagonal) => isDiagonal 
      ? Diagonal * speed : Straight * speed;

    public override string ToString() => $"{nameof(StartingCoord)}: {StartingCoord}, {nameof(TakenCoord)}: {TakenCoord}, {nameof(Coord)}: {Coord}, {nameof(speed)}: {speed}";
    
    static F32 Straight => One;
    static F32 Diagonal => Sqrt(Two);
    readonly F32 speed;
  }
}