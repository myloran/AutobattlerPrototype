using Model.NBattleSimulation;
using Shared;
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
    
    readonly PlayerContext playerContext;
    readonly PlayerPresenterContext playerPresenterContext;
    readonly BattleSetupUI battleSetupUI;
  }
}