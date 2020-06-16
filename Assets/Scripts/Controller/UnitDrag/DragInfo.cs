using Shared;
using View.Views;

namespace Controller.UnitDrag {
  public struct DragInfo {
    public UnitView Unit;
    public Coord StartCoord;
    
    public DragInfo(UnitView unit, Coord startCoord) {
      Unit = unit;
      StartCoord = startCoord;
    }
  }
}