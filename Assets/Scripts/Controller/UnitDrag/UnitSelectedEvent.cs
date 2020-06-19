using Shared;
using View.NUnit;

namespace Controller.UnitDrag {
  public struct UnitSelectedEvent {
    public UnitView Unit;
    public Coord StartCoord;
    
    public UnitSelectedEvent(UnitView unit, Coord startCoord) {
      Unit = unit;
      StartCoord = startCoord;
    }
  }
}