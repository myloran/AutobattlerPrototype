using System;
using Model.NBattleSimulation;
using View;
using View.UI;

namespace Controller {
  public class BattleSetupController : IDisposable {
    public BattleSetupController(Player[] players, BenchView[] benches, 
        BattleSetupUI ui) {
      this.players = players;
      this.benches = benches;
      this.ui = ui;
      ui.BAdd.onClick.AddListener(AddUnit);
      ui.BRemove.onClick.AddListener(RemoveUnit);
    }

    void AddUnit() {
      var playerId = ui.GetSelectedPlayerId;
      var name = ui.GetSelectedUnitName;
      var (isAdded, coord) = benches[playerId].AddUnit(name);
      if (isAdded) players[playerId].AddBenchUnit(name, coord, playerId); 
    }
    
    void RemoveUnit() {
      var id = ui.GetSelectedPlayerId;
      var (isRemoved, coord) = benches[id].RemoveUnit();
      if (isRemoved) players[id].RemoveBenchUnit(coord);
    }

    public void Dispose() {
      ui.BAdd.onClick.RemoveListener(AddUnit);
      ui.BRemove.onClick.RemoveListener(RemoveUnit);
    }

    readonly BattleSetupUI ui;
    readonly BenchView[] benches;
    readonly Player[] players;
  }
}