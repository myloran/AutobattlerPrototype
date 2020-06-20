using Shared;
using Shared.Abstraction;
using View.Exts;
using View.NUnit;
using View.Presenters;

namespace View {
  public class UnitViewCoordChangedHandler : IHandler<UnitCoordChanged<UnitView>> {
    public UnitViewCoordChangedHandler(TilePresenter tilePresenter) {
      this.tilePresenter = tilePresenter;
    }
    
    public void Handle(UnitCoordChanged<UnitView> e) {
      var toPosition = tilePresenter.PositionAt(e.To).WithY(e.Unit.Height);
      e.Unit.transform.position = toPosition;
    }

    readonly TilePresenter tilePresenter;
  }
}