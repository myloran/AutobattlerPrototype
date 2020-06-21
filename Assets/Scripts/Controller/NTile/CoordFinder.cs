using Controller.NBattleSimulation;
using Shared;
using Shared.Poco;
using UnityEngine;
using View.NTile;
using View.NUnit;
using View.Presenters;
using View.UIs;
using View.Views;

namespace Controller.NTile {
  public class CoordFinder { //TODO:rename to CoordFinderBySelectedPlayer, create in drag controller constructor
    //TODO: extract interface?
    public CoordFinder(TilePresenter tilePresenter, BattleSetupUI battleSetupUI) {
      this.tilePresenter = tilePresenter;
      this.battleSetupUI = battleSetupUI;
    }

    public Coord Find(Vector3 mousePosition) {
      var player = (EPlayer)battleSetupUI.GetSelectedPlayerId;
      return tilePresenter.FindClosestCoordLimitedByPlayerSide(mousePosition, player);
    }
    
    public Coord FindClosest(UnitView unit) => 
      tilePresenter.FindClosestCoord(unit.transform.position, unit.Player);
    
    readonly TilePresenter tilePresenter;
    readonly BattleSetupUI battleSetupUI;
  }
}