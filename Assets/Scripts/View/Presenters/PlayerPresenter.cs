using Shared;
using View.Exts;

namespace View.Presenters {
  public class PlayerPresenter : BasePlayer<UnitView> {
    public PlayerPresenter(IUnitDict<UnitView> boardUnitDict, 
        IUnitDict<UnitView> benchUnitDict, TilePresenter tilePresenter) 
        : base(boardUnitDict, benchUnitDict) {
      this.tilePresenter = tilePresenter;
    }

    protected override void OnChangeCoord(Coord coord, UnitView unit) {
      var toPosition = tilePresenter.PositionAt(coord).WithY(unit.Height);
      unit.transform.position = toPosition;
    }

    readonly TilePresenter tilePresenter;
  }
}