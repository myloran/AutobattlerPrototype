namespace Shared {
  public struct UnitCoordChanged<T> {
    public T Unit;
    public Coord To;

    public UnitCoordChanged(T unit, Coord to) {
      Unit = unit;
      To = to;
    }
  }
}