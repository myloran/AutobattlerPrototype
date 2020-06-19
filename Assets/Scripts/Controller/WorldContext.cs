using Model.NBattleSimulation;
using Shared;
using View.Presenters;
using View.UIs;

namespace Controller {
  public class WorldContext {
    public WorldContext(Player[] players, PlayerPresenter[] playerPresenters, BattleSetupUI battleSetupUI) {
      this.players = players;
      this.playerPresenters = playerPresenters;
      this.battleSetupUI = battleSetupUI;
    }
    
    public void Move(Coord from, Coord to) {
      var selectedPlayerId = battleSetupUI.GetSelectedPlayerId;
      var player = players[selectedPlayerId];
      player.MoveUnit(from, to);
      
      var playerPresenter = playerPresenters[selectedPlayerId];
      playerPresenter.MoveUnit(from, to);
    }
    
    readonly Player[] players;
    readonly PlayerPresenter[] playerPresenters;
    readonly BattleSetupUI battleSetupUI;
  }
}