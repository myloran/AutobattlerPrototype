using Shared.Poco;

namespace Shared {
  public struct UnitCoordChanged<T> {
    public readonly T Unit;
    public Coord To;

    public UnitCoordChanged(T unit, Coord to) {
      Unit = unit;
      To = to;
    }
  }
}