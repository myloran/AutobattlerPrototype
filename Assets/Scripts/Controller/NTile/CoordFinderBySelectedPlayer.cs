using Controller.NBattleSimulation;
using Shared;
using Shared.Primitives;
using UnityEngine;
using View.NTile;
using View.Presenters;
using View.UIs;
using View.Views;

namespace Controller.NTile {
  public class CoordFinderBySelectedPlayer {
    public CoordFinderBySelectedPlayer(CoordFinder coordFinder, BattleSetupUI battleSetupUI) {
      this.coordFinder = coordFinder;
      this.battleSetupUI = battleSetupUI;
    }

    public Coord Find(Vector3 mousePosition) {
      var player = (EPlayer)battleSetupUI.GetSelectedPlayerId;
      return coordFinder.FindClosestCoordLimitedByPlayerSide(mousePosition, player);
    }

    readonly CoordFinder coordFinder;
    readonly BattleSetupUI battleSetupUI;
  }
}