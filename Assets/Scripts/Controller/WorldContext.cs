using Model.NBattleSimulation;
using Shared;
using View.Presenters;
using View.UIs;

namespace Controller {
  public class WorldContext {
    public WorldContext(PlayerContext playerContext, PlayerPresenter[] playerPresenters, BattleSetupUI battleSetupUI) {
      this.playerContext = playerContext;
      this.playerPresenters = playerPresenters;
      this.battleSetupUI = battleSetupUI;
    }
    
    public void Move(Coord from, Coord to) {
      var selectedPlayerId = battleSetupUI.GetSelectedPlayerId;
      var player = (EPlayer) selectedPlayerId;
      playerContext.MoveUnit(from, to, player);
      
      var playerPresenter = playerPresenters[selectedPlayerId];
      playerPresenter.MoveUnit(from, to);
    }
    
    readonly PlayerContext playerContext;
    readonly PlayerPresenter[] playerPresenters;
    readonly BattleSetupUI battleSetupUI;
  }
}