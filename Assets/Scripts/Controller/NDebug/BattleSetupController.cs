using System;
using Model.NBattleSimulation;
using Shared;
using Shared.Poco;
using View.Presenters;
using View.UIs;

namespace Controller.NDebug {
  public class BattleSetupController : IDisposable {
    public BattleSetupController(PlayerContext playerContext, PlayerPresenterContext playerPresenterContext, 
        BattleSetupUI ui) {
      this.playerContext = playerContext;
      this.playerPresenterContext = playerPresenterContext;
      this.ui = ui;
      ui.BAdd.onClick.AddListener(AddUnit); //TODO: move to unirx
      ui.BRemove.onClick.AddListener(RemoveUnit);
    }

    void AddUnit() {
      var playerId = ui.GetSelectedPlayerId;
      var name = ui.GetSelectedUnitName;
      //TODO: use PlayerSharedContext
      var (isInstantiated, coord) = playerContext.InstantiateToBenchStart(name, (EPlayer)playerId); 
      if (isInstantiated) playerPresenterContext.InstantiateToBench(name, coord, (EPlayer)playerId);
    }
    
    void RemoveUnit() {
      var id = (EPlayer)ui.GetSelectedPlayerId;
      //TODO: use PlayerSharedContext
      var (isDestroyed, coord) = playerContext.DestroyFromBenchEnd(id);
      if (isDestroyed) playerPresenterContext.DestroyFromBench(id, coord);
    }

    public void Dispose() {
      ui.BAdd.onClick.RemoveListener(AddUnit);
      ui.BRemove.onClick.RemoveListener(RemoveUnit);
    }

    readonly BattleSetupUI ui;
    readonly PlayerPresenterContext playerPresenterContext;
    readonly PlayerContext playerContext;
  }
}