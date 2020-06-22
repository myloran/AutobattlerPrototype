using System;
using MessagePack;
using static Shared.Const;
using static UnityEngine.Mathf;

namespace Shared.Primitives {
  [Serializable]
  [MessagePackObject]
  public struct Coord : IEquatable<Coord> {
    public static Coord Invalid = new Coord(-1, -1);
    [Key(0)] public int X; 
    [Key(1)] public int Y;
    
    public Coord(int x, int y) {
      X = x;
      Y = y;
    }
    
    public bool Equals(Coord other) => X == other.X && Y == other.Y;
    public override bool Equals(object obj) => obj is Coord other && Equals(other);

    public override int GetHashCode() {
      unchecked {
        return (X * 397) ^ Y;
      }
    }

    [IgnoreMember] public bool IsDiagonal => X != 0 && Y != 0;
    [IgnoreMember] public int SqrMagnitude => X * X + Y * Y; 
    [IgnoreMember] public Coord Normalized => new Coord(Clamp(X, -1, 1), Clamp(Y, -1, 1));

    public static implicit operator (int x, int y)(Coord coord) => (coord.X, coord.Y);
    public static implicit operator Coord ((int x, int y) coord) => new Coord(coord.x, coord.y);

    public static Coord operator +(Coord a, Coord b) => new Coord(a.X + b.X, a.Y + b.Y);
    public static Coord operator -(Coord a, Coord b) => new Coord(a.X - b.X, a.Y - b.Y);
    public static bool operator ==(Coord a, Coord b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Coord a, Coord b) => a.X != b.X || a.Y != b.Y;
    public static bool operator ==(Coord a, (int x, int y) b) => a.X == b.x && a.Y == b.y;
    public static bool operator !=(Coord a, (int x, int y) b) => a.X != b.x || a.Y != b.y;

    public override string ToString() => $"{nameof(X)}:{X}, {nameof(Y)}:{Y}";
  }

  public static class CoordExt {
    public static bool IsInsideBoard(this Coord coord) =>
      coord.X >= 0 && coord.X <= 7 && coord.Y >= 0 && coord.Y <= 5;
    
    public static (Coord, Coord) GetClosestDirections(this Coord direction) {
      if (direction.IsDiagonal) return ((direction.X, 0), (0, direction.Y));

      return direction.X == 0 
        ? ((1, direction.Y), (-1, direction.Y)) 
        : ((direction.X, 1), (direction.X, -1));
    }
        
    public static Coord GetClosestDirection(this Coord direction, Coord excludedDirection) {
      var (direction1, direction2) = GetClosestDirections(direction);
      return excludedDirection == direction1 ? direction2 : direction1;
    }
    
    public static Coord LimitByPlayerSide(this Coord coord, EPlayer player) {
      coord.Y = player == EPlayer.First
        ? Clamp(coord.Y, 0, 2)
        : Clamp(coord.Y, 3, 5);
      return coord;
    }

    public static bool IsPlayer1Bench(this Coord coord) => coord.Y == Player1BenchId;
    public static bool IsPlayer2Bench(this Coord coord) => coord.Y == Player2BenchId;
    
    public static bool BelongsToPlayer(this Coord coord, EPlayer player) => 
      coord.Y == player.BenchId();

    public static Coord Diff(this Coord self, Coord to) {
      var minX = self.X > to.X ? self.X - to.X : to.X - self.X;
      var minY = self.Y > to.Y ? self.Y - to.Y : to.Y - self.Y;
      
      return new Coord(minX, minY);
    }
    
    public static int SqrDistance(Coord self, Coord to) => Diff(self, to).SqrMagnitude; 
  }
}