using Controller.NUnit;
using Model.NBattleSimulation;
using Shared.Primitives;
using View.Presenters;
using View.UIs;

namespace Controller {
  public class PlayerSharedContext {
    public PlayerSharedContext(PlayerContext playerContext, PlayerPresenterContext playerPresenterContext, BattleSetupUI battleSetupUI) {
      this.playerContext = playerContext;
      this.playerPresenterContext = playerPresenterContext;
      this.battleSetupUI = battleSetupUI;
    }
    
    public void Move(Coord from, Coord to) {
      var player = (EPlayer) battleSetupUI.GetSelectedPlayerId;
      playerContext.MoveUnit(from, to, player);
      playerPresenterContext.MoveUnit(from, to, player);
    }

    public UnitContext InstantiateToBoard(string name, Coord coord, EPlayer player) {
      var unitModel = playerContext.InstantiateToBoard(name, coord, player);
      var unitView = playerPresenterContext.InstantiateToBoard(name, coord, player);
      return new UnitContext(unitModel, unitView);
    }

    public void DestroyAll() {
      playerContext.DestroyAll();
      playerPresenterContext.DestroyAll();
    }
    
    readonly PlayerContext playerContext;
    readonly PlayerPresenterContext playerPresenterContext;
    readonly BattleSetupUI battleSetupUI;
  }
}