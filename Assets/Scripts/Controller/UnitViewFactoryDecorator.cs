using Controller.NBattleSimulation;
using Model.NBattleSimulation;
using Shared;
using UnityEngine;
using View;
using View.Presenters;
using View.UI;

namespace Controller {
  //TODO: Move away from unit view, make separate controller
  public class UnitViewFactoryDecorator : IUnitViewFactory {
    public UnitViewFactoryDecorator(TilePresenter tilePresenter,
        BattleSetupUI battleSetupUI, Player[] players, PlayerPresenter[] playerPresenters, 
        UnitTooltipController unitTooltipController, IUnitViewFactory decorator,
        BattleStateController battleStateController) {
      this.tilePresenter = tilePresenter;
      this.battleSetupUI = battleSetupUI;
      this.players = players;
      this.playerPresenters = playerPresenters;
      this.unitTooltipController = unitTooltipController;
      this.decorator = decorator;
      this.battleStateController = battleStateController;
    }
    
    public UnitView Create(string name, Coord coord, EPlayer player) {
      var unit = decorator.Create(name, coord, player);
      unit.GetComponent<UnitDragController>().Init(tilePresenter, battleSetupUI, 
        players, playerPresenters, unitTooltipController, unit, battleStateController);
      return unit;
    }

    readonly TilePresenter tilePresenter;
    readonly IUnitViewFactory decorator;
    readonly BattleStateController battleStateController;
    readonly BattleSetupUI battleSetupUI;
    readonly Player[] players;
    readonly PlayerPresenter[] playerPresenters;
    readonly UnitTooltipController unitTooltipController;
  }
}