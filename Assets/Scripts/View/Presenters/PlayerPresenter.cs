using Shared;
using Shared.Abstraction;
using View.Exts;
using View.Views;

namespace View.Presenters {
  public class PlayerPresenter : BasePlayer<UnitView> {
    public PlayerPresenter(EPlayer player, IUnitDict<UnitView> boardUnitDict, 
        IUnitDict<UnitView> benchUnitDict, TilePresenter tilePresenter) 
        : base(player, boardUnitDict, benchUnitDict) {
      this.tilePresenter = tilePresenter;
    }

    protected override void OnChangeCoord(Coord coord, UnitView unit) {
      var toPosition = tilePresenter.PositionAt(coord).WithY(unit.Height);
      unit.transform.position = toPosition;
    }

    readonly TilePresenter tilePresenter;
  }
}