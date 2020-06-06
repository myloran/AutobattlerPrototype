using System;
using Model.NBattleSimulation;
using Shared;
using View;
using View.Presenters;
using View.UI;

namespace Controller {
  public class BattleSetupController : IDisposable {
    public BattleSetupController(Player[] players, PlayerPresenter[] presenters, 
        BattleSetupUI ui) {
      this.players = players;
      this.presenters = presenters;
      this.ui = ui;
      ui.BAdd.onClick.AddListener(AddUnit);
      ui.BRemove.onClick.AddListener(RemoveUnit);
    }

    void AddUnit() {
      var playerId = ui.GetSelectedPlayerId;
      var name = ui.GetSelectedUnitName;
      var (isAdded, coord) = players[playerId].BenchUnits
        .InstantiateToStart(name, (EPlayer)playerId); 
      if (isAdded) presenters[playerId].BenchUnits
        .Instantiate(name, coord, (EPlayer)playerId);
    }
    
    void RemoveUnit() {
      var id = ui.GetSelectedPlayerId;
      presenters[id].BenchUnits.DestroyFromEnd((EPlayer)id);
      players[id].BenchUnits.DestroyFromEnd((EPlayer)id);
    }

    public void Dispose() {
      ui.BAdd.onClick.RemoveListener(AddUnit);
      ui.BRemove.onClick.RemoveListener(RemoveUnit);
    }

    readonly BattleSetupUI ui;
    readonly PlayerPresenter[] presenters;
    readonly Player[] players;
  }
}