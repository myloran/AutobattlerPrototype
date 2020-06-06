using Controller.NDebug;
using Model.NBattleSimulation;
using Shared;
using UniRx;
using UnityEngine;
using View;
using View.Presenters;
using View.UI;

namespace Controller.NBattleSimulation {
  public class UnitDragController {
    public UnitDragController(TilePresenter tilePresenter, BattleSetupUI battleSetupUI,
        Player[] players, PlayerPresenter[] playerPresenters, 
        UnitTooltipController unitTooltipController, 
        BattleStateController battleStateController, RaycastController raycastController, UnitModelDebugController unitModelDebugController) {
      this.tilePresenter = tilePresenter;
      this.battleSetupUI = battleSetupUI;
      this.players = players;
      this.playerPresenters = playerPresenters;
      this.unitTooltipController = unitTooltipController;
      this.battleStateController = battleStateController;
      this.raycastController = raycastController;
      this.unitModelDebugController = unitModelDebugController;
    }
    
    public void Init() {
      raycastController.OnUnitHit.Subscribe(StartDrag);

      Observable.EveryUpdate()
        .Where(_ => Input.GetMouseButton(0)) 
        .Subscribe(_ => Drag());
      
      Observable.EveryUpdate()
        .Where(_ => Input.GetMouseButtonUp(0))
        .Where(_ => isDragging)
        .Subscribe(_ => EndDrag());
    }

    void StartDrag(RaycastHit hit) {
      unit = hit.transform.GetComponent<UnitView>();
      startCoord = tilePresenter.FindClosestCoord(unit.transform.position, unit.Player);
      
      if (battleStateController.IsBattleStarted) {
          unitModelDebugController.SelectUnitModel(startCoord);
          // unitTooltipController.Show(unit.Info);
        return;
      }
      if (unit.Player != (EPlayer)battleSetupUI.GetSelectedPlayerId) return;
      
      isDragging = true;
    }
    
    void EndDrag() {
      if (battleStateController.IsBattleStarted) return;
      var selectedPlayerId = battleSetupUI.GetSelectedPlayerId;
      if (unit.Player != (EPlayer)selectedPlayerId) return;
      
      isDragging = false;
      tilePresenter.TileAt(lastCoord).Unhighlight();
      
      var player = players[selectedPlayerId];
      player.MoveUnit(startCoord, lastCoord);
      
      var playerPresenter = playerPresenters[selectedPlayerId];
      playerPresenter.MoveUnit(startCoord, lastCoord);
      
      lastCoord = Coord.Invalid;
    }

    void Drag() {
      if (battleStateController.IsBattleStarted) return;
      if (!isDragging) return;

      var (isHit, mousePosition) = raycastController.RaycastPlane();
      if (!isHit) return;
      
      unit.transform.position = mousePosition + new Vector3(0, 1, 0);

      var coord = tilePresenter.FindClosestCoord(mousePosition, (EPlayer)battleSetupUI.GetSelectedPlayerId);
      
      if (coord != lastCoord) {
        if (lastCoord != Coord.Invalid)
          tilePresenter.TileAt(lastCoord).Unhighlight();
        
        tilePresenter.TileAt(coord).Highlight();
        //   //if swap previous, cancel it
        //   if (tile.Unit != null) {
        //     SwapUnits(tile, oldTile);
        //   }
        //   //swap units if tile with unit
      }

      lastCoord = coord;
    }

    readonly RaycastController raycastController;
    readonly TilePresenter tilePresenter;
    readonly BattleSetupUI battleSetupUI;
    readonly Player[] players;
    readonly PlayerPresenter[] playerPresenters;
    readonly UnitTooltipController unitTooltipController;
    readonly BattleStateController battleStateController;
    readonly UnitModelDebugController unitModelDebugController;
    bool isDragging;
    Coord startCoord;
    Coord lastCoord = Coord.Invalid;
    UnitView unit;
  }
}