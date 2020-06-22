using Shared;
using Shared.Primitives;

namespace Controller.UnitDrag {
  public struct CoordChangedEvent {
    public Coord From;
    public Coord To;

    public CoordChangedEvent(Coord @from, Coord to) {
      From = @from;
      To = to;
    }
  }
}