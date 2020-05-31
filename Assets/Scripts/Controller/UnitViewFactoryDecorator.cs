using Controller.BattleSimulation;
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
        UnitTooltipController unitTooltipController, IUnitViewFactory decorator) {
      this.closestTileFinder = closestTileFinder;
      this.battleSetupUI = battleSetupUI;
      this.players = players;
      this.unitTooltipController = unitTooltipController;
      this.decorator = decorator;
    }
    
    public UnitView Create(string name, Vector3 position, TileView tile, EPlayer player) {
      var unit = decorator.Create(name, position, tile, player);
      unit.GetComponent<UnitDragController>().Init(closestTileFinder, battleSetupUI, 
        players, unitTooltipController, unit);
      return unit;
    }

    readonly ClosestTileFinder closestTileFinder;
    readonly IUnitViewFactory decorator;
    BattleSetupUI battleSetupUI;
    Player[] players;
    readonly UnitTooltipController unitTooltipController;
  }
}