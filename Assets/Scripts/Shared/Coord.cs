using System;
using MessagePack;

namespace Shared {
  [Serializable]
  [MessagePackObject]
  public class Coord {
    public static Coord Invalid = new Coord(-1, -1);
    [Key(0)] public int X; 
    [Key(1)] public int Y;
    
    public Coord(int x, int y) {
      X = x;
      Y = y;
    }
    
    public bool Equals(Coord other) {
      return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj) {
      return obj is Coord other && Equals(other);
    }

    public override int GetHashCode() {
      unchecked {
        return (X * 397) ^ Y;
      }
    }

    [IgnoreMember] public float SqrMagnitude => X * X + Y * Y; 
    [IgnoreMember] public Coord Normalized => new Coord(X > 0 ? 1 : 0, Y > 0 ? 1 : 0);

    public static Coord operator +(Coord a, Coord b) => new Coord(a.X + b.X, a.Y + b.Y);
    public static Coord operator -(Coord a, Coord b) => new Coord(a.X - b.X, a.Y - b.Y);

    // public override string ToString() => $"{nameof(X)}:{X}, {nameof(Y)}:{Y}";
  }

  public static class CoordExt {
    public static Coord Diff(this Coord self, Coord to) {
      var minX = self.X > to.X ? self.X - to.X : to.X - self.X;
      var minY = self.Y > to.Y ? self.Y - to.Y : to.Y - self.Y;
      
      return new Coord(minX, minY);
    }
    
    public static float SqrDistance(this Coord self, Coord to) => Diff(self, to).SqrMagnitude;
  }
}