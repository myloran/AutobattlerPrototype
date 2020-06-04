using Controller.NBattleSimulation;
using Model;
using Model.NBattleSimulation;
using Shared;
using UnityEngine;
using View;
using View.UI;

namespace Controller {
  public class UnitViewFactoryDecorator : IUnitViewFactory {
    public UnitViewFactoryDecorator(ClosestTileFinder closestTileFinder,
        BattleSetupUI battleSetupUI, Player[] players, 
        UnitTooltipController unitTooltipController, IUnitViewFactory decorator,
        BattleStateController battleStateController) {
      this.closestTileFinder = closestTileFinder;
      this.battleSetupUI = battleSetupUI;
      this.players = players;
      this.unitTooltipController = unitTooltipController;
      this.decorator = decorator;
      this.battleStateController = battleStateController;
    }
    
    public UnitView Create(string name, Vector3 position, TileView tile, EPlayer player) {
      var unit = decorator.Create(name, position, tile, player);
      unit.GetComponent<UnitDragController>().Init(closestTileFinder, battleSetupUI, 
        players, unitTooltipController, unit, battleStateController);
      return unit;
    }

    readonly ClosestTileFinder closestTileFinder;
    readonly IUnitViewFactory decorator;
    readonly BattleStateController battleStateController;
    readonly BattleSetupUI battleSetupUI;
    readonly Player[] players;
    readonly UnitTooltipController unitTooltipController;
  }
}