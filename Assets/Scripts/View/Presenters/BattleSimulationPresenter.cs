using Shared;
using Shared.Exts;
using Shared.Shared.Client;
using Shared.Shared.Client.Abstraction;
using View.Exts;
using View.NTile;

namespace View.Presenters {
  public class BattleSimulationPresenter { //TODO: implement ISimulationTick?
    public BattleSimulationPresenter(TilePresenter tile, BoardPresenter board, 
        ISimulationTick view) {
      this.tile = tile;
      this.board = board;
      this.view = view;
    }
    
    public void Reset(PlayerPresenterContext context) {
      foreach (var (coord, unit) in context.BoardUnits()) {
        var position = tile.PositionAt(coord).WithY(unit.Height);
        unit.ResetState(position);
      }
      board.Reset(context.BoardUnits());
    }
    
    public void SimulationTick(float time) => view.SimulationTick(time);

    readonly BoardPresenter board;
    readonly TilePresenter tile;
    readonly ISimulationTick view;
  }
}