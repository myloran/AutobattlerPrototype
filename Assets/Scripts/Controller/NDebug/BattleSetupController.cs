using System;
using Model.NBattleSimulation;
using Shared;
using View.Presenters;
using View.UIs;

namespace Controller.NDebug {
  public class BattleSetupController : IDisposable {
    public BattleSetupController(PlayerContext playerContext, PlayerPresenter[] presenters, 
        BattleSetupUI ui) {
      this.playerContext = playerContext;
      this.presenters = presenters;
      this.ui = ui;
      ui.BAdd.onClick.AddListener(AddUnit);
      ui.BRemove.onClick.AddListener(RemoveUnit);
    }

    void AddUnit() {
      var playerId = ui.GetSelectedPlayerId;
      var name = ui.GetSelectedUnitName;
      var (isInstantiated, coord) = playerContext.InstantiateToBenchStart(name, (EPlayer)playerId); 
      if (isInstantiated) presenters[playerId].InstantiateToBench(name, coord, (EPlayer)playerId);
    }
    
    void RemoveUnit() {
      var id = ui.GetSelectedPlayerId;
      var (isDestroyed, coord) = playerContext.DestroyFromBenchEnd((EPlayer)id);
      if (isDestroyed) presenters[id].DestroyOnBench(coord);
    }

    public void Dispose() {
      ui.BAdd.onClick.RemoveListener(AddUnit);
      ui.BRemove.onClick.RemoveListener(RemoveUnit);
    }

    readonly BattleSetupUI ui;
    readonly PlayerPresenter[] presenters;
    readonly PlayerContext playerContext;
  }
}