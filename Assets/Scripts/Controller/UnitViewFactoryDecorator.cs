using Model;
using UnityEngine;
using View;
using View.UI;

namespace Controller {
  public class UnitViewFactoryDecorator : IUnitViewFactory {
    public UnitViewFactoryDecorator(ClosestTileFinder closestTileFinder, 
        BattleSetupUI battleSetupUI, Player[] players, IUnitViewFactory decorator) {
      this.closestTileFinder = closestTileFinder;
      this.battleSetupUI = battleSetupUI;
      this.players = players;
      this.decorator = decorator;
    }
    
    public UnitView Create(string name, Vector3 position, TileView tile) {
      var unit = decorator.Create(name, position, tile);
      unit.GetComponent<UnitDragController>().Init(closestTileFinder, battleSetupUI, players, unit);
      return unit;
    }

    readonly ClosestTileFinder closestTileFinder;
    readonly IUnitViewFactory decorator;
    BattleSetupUI battleSetupUI;
    Player[] players;
  }
}