using Shared;

namespace Controller.UnitDrag {
  public struct EndDragEvent {
    public Coord Start;
    public Coord Last;

    public EndDragEvent(Coord start, Coord last) {
      Start = start;
      Last = last;
    }
  }
}