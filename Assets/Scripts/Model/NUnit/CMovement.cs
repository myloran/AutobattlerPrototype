using System;
using Shared;

namespace Model.NUnit {
  public class CMovement {
    public static float Straight => 1;
    public static float Diagonal => (float)Math.Sqrt(2);
    public Coord StartingCoord;
    public Coord TakenCoord = Coord.Invalid;
    public Coord Coord;
    
    public CMovement(Coord coord, float speed) {
      StartingCoord = coord;
      this.speed = speed;
    }

    public void Reset() {
      Coord = StartingCoord;
    }
    
    public float TimeToMove(bool isDiagonal = true) => isDiagonal 
      ? Diagonal * speed : Straight * speed;

    readonly float speed;
  }
}