using Shared.Abstraction;
using Shared.Exts;
using SharedClient.Abstraction;
using View.Exts;
using View.NTile;

namespace View.Presenters {
  public class BattleSimulationPresenter : ISimulationTick {
    public BattleSimulationPresenter(CoordFinder tile, BoardPresenter board,
      ISimulationTick tickables, IReset resettables) {
      this.tile = tile;
      this.board = board;
      this.tickables = tickables;
      this.resettables = resettables;
    }
    
    public void Reset(PlayerPresenterContext context) {
      resettables.Reset();
      var boardUnits = context.BoardUnits();
      foreach (var (coord, unit) in boardUnits) {
        var position = tile.PositionAt(coord).WithY(unit.Height);
        unit.ResetState(position);
      }
      board.SetUnits(boardUnits);
    }
    
    public void SimulationTick(float time) => tickables.SimulationTick(time);

    readonly BoardPresenter board;
    readonly CoordFinder tile;
    readonly ISimulationTick tickables;
    readonly IReset resettables;
  }
}