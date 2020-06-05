using Shared;
using View.Exts;

namespace View.Presenters {
  public class PlayerPresenter : BasePlayer<UnitView> {
    public void Init(TilePresenter tilePresenter, IUnitDict<UnitView> boardUnitDict,
        IUnitDict<UnitView> benchUnitDict) {
      this.tilePresenter = tilePresenter;
      Init(boardUnitDict, benchUnitDict);
    }
    
    protected override void OnChangeCoord(Coord coord, UnitView unit) {
      var toPosition = tilePresenter.PositionAt(coord).WithY(unit.Height);
      unit.transform.position = toPosition;
    }

    TilePresenter tilePresenter;
  }
}