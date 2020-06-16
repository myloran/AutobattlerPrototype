using Shared;
using Shared.Abstraction;
using View.Exts;
using View.Views;

namespace View.Presenters {
  public class BoardPresenter : BaseBoard<UnitView, PlayerPresenter> {
    public BoardPresenter(IUnitDict<UnitView> units, IUnitDict<UnitView> player1Units, 
      IUnitDict<UnitView> player2Units, TilePresenter tilePresenter) : base(units, player1Units, player2Units) {
      this.tilePresenter = tilePresenter;
    }
    
    protected override void OnChangeCoord(Coord coord, UnitView unit) {
      var toPosition = tilePresenter.PositionAt(coord).WithY(unit.Height);
      unit.transform.position = toPosition;
    }

    readonly TilePresenter tilePresenter;
  }
}