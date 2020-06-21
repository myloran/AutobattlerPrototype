using Shared;
using Shared.Abstraction;
using View.Exts;
using View.NTile;

namespace View.NUnit {
  public class UnitViewCoordChangedHandler : IHandler<UnitCoordChanged<UnitView>> {
    public UnitViewCoordChangedHandler(CoordFinder coordFinder) => 
      this.coordFinder = coordFinder;

    public void Handle(UnitCoordChanged<UnitView> e) {
      var toPosition = coordFinder.PositionAt(e.To).WithY(e.Unit.Height);
      e.Unit.transform.position = toPosition;
    }

    readonly CoordFinder coordFinder;
  }
}