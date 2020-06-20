using System.Collections.Generic;
using Shared;
using Shared.Shared.Client;
using View.Exts;
using View.NUnit;

namespace View.Presenters {
  public class BattleSimulationPresenter {
    public BattleSimulationPresenter(TilePresenter tilePresenter, BoardPresenter boardPresenter, 
        ISimulationTick viewSimulation) {
      this.tilePresenter = tilePresenter;
      this.boardPresenter = boardPresenter;
      this.viewSimulation = viewSimulation;
    }
    
    public void Reset(PlayerPresenter[] playerPresenters) {
      foreach (var (coord, unit) in units) {
        unit.transform.position = tilePresenter.PositionAt(coord).WithY(unit.Height);
        unit.ResetHealth();
        unit.Show();
      }
      boardPresenter.Reset(playerPresenters[0], playerPresenters[1]);
      foreach (var (coord, unit) in playerPresenters[0].BoardUnits) units[coord] = unit;
      foreach (var (coord, unit) in playerPresenters[1].BoardUnits) units[coord] = unit;
    }
    
    public void SimulationTick(float time) => viewSimulation.SimulationTick(time);

    readonly BoardPresenter boardPresenter;
    readonly TilePresenter tilePresenter;
    readonly ISimulationTick viewSimulation;
    readonly Dictionary<Coord, UnitView> units = new Dictionary<Coord, UnitView>();
  }
}