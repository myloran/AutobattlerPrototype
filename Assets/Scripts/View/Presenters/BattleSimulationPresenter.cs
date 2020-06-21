using Shared;
using Shared.Exts;
using Shared.Shared.Client;
using Shared.Shared.Client.Abstraction;
using View.Exts;
using View.NTile;

namespace View.Presenters {
  public class BattleSimulationPresenter : ISimulationTick {
    public BattleSimulationPresenter(CoordFinder tile, BoardPresenter board, 
        ISimulationTick view) {
      this.tile = tile;
      this.board = board;
      this.view = view;
    }
    
    public void Reset(PlayerPresenterContext context) {
      var boardUnits = context.BoardUnits();
      foreach (var (coord, unit) in boardUnits) {
        var position = tile.PositionAt(coord).WithY(unit.Height);
        unit.ResetState(position);
      }
      board.Reset(boardUnits);
    }
    
    public void SimulationTick(float time) => view.SimulationTick(time);

    readonly BoardPresenter board;
    readonly CoordFinder tile;
    readonly ISimulationTick view;
  }
}