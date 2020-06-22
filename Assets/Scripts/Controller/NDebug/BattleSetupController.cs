using System;
using Controller.Exts;
using Model.NBattleSimulation;
using Shared;
using Shared.Primitives;
using View.Presenters;
using View.UIs;

namespace Controller.NDebug {
  public class BattleSetupController {
    public BattleSetupController(PlayerContext playerContext, PlayerPresenterContext playerPresenterContext, 
        BattleSetupUI ui) {
      this.playerContext = playerContext;
      this.playerPresenterContext = playerPresenterContext;
      this.ui = ui;
    }

    public void SubToUI() {
      ui.BAdd.Sub(AddUnit);
      ui.BRemove.Sub(RemoveUnit);
    }

    void AddUnit() {
      var playerId = ui.GetSelectedPlayerId;
      var name = ui.GetSelectedUnitName;
      var (isInstantiated, coord) = playerContext.InstantiateToBenchStart(name, (EPlayer)playerId); 
      if (isInstantiated) playerPresenterContext.InstantiateToBench(name, coord, (EPlayer)playerId);
    }
    
    void RemoveUnit() {
      var id = (EPlayer)ui.GetSelectedPlayerId;
      var (isDestroyed, coord) = playerContext.DestroyFromBenchEnd(id);
      if (isDestroyed) playerPresenterContext.DestroyFromBench(id, coord);
    }

    readonly BattleSetupUI ui;
    readonly PlayerPresenterContext playerPresenterContext;
    readonly PlayerContext playerContext;
  }
}