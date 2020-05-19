namespace Shared {
  public struct Coord {
    public int X, Y;
    
    public Coord(int x, int y) {
      X = x;
      Y = y;
    }

    public override string ToString() => $"{nameof(X)}:{X}, {nameof(Y)}:{Y}";
  }
}