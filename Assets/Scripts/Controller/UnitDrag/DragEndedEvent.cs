using Shared;
using Shared.Primitives;

namespace Controller.UnitDrag {
  public struct DragEndedEvent {
    public Coord Start;
    public Coord Last;

    public DragEndedEvent(Coord start, Coord last) {
      Start = start;
      Last = last;
    }
  }
}