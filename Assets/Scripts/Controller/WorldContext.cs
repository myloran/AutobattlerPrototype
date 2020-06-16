using Controller.UnitDrag;
using Model.NBattleSimulation;
using View.Presenters;
using View.UIs;

namespace Controller {
  public class WorldContext : IHandler<EndDragEvent> {
    public WorldContext(Player[] players, PlayerPresenter[] playerPresenters, BattleSetupUI battleSetupUI) {
      this.players = players;
      this.playerPresenters = playerPresenters;
      this.battleSetupUI = battleSetupUI;
    }
    
    public void Handle(EndDragEvent e) {
      var selectedPlayerId = battleSetupUI.GetSelectedPlayerId;
      var player = players[selectedPlayerId];
      player.MoveUnit(e.Start, e.Last);
      
      var playerPresenter = playerPresenters[selectedPlayerId];
      playerPresenter.MoveUnit(e.Start, e.Last);
    }
    
    readonly Player[] players;
    readonly PlayerPresenter[] playerPresenters;
    readonly BattleSetupUI battleSetupUI;
  }
}