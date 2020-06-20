using Shared;
using Shared.Poco;
using View.NUnit;

namespace Controller.NUnit {
  public struct UnitSelectedEvent {
    public readonly UnitView Unit;
    public Coord StartCoord;
    
    public UnitSelectedEvent(UnitView unit, Coord startCoord) {
      Unit = unit;
      StartCoord = startCoord;
    }
  }
}