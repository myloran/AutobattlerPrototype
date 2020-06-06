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
        BattleStateController battleStateController, RaycastController raycastController, 
        Camera cam) {
      this.tilePresenter = tilePresenter;
      this.battleSetupUI = battleSetupUI;
      this.players = players;
      this.playerPresenters = playerPresenters;
      this.unitTooltipController = unitTooltipController;
      this.battleStateController = battleStateController;
      this.raycastController = raycastController;
      this.cam = cam;
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
      if (battleStateController.IsBattleStarted) {
        // if (isDebug)
        //   unitDebugController.Show();
        // else
        unitTooltipController.Show(unit.Info);
        return;
      }
      if (unit.Player != (EPlayer)battleSetupUI.GetSelectedPlayerId) return;
      
      isDragging = true;
            
      if (lastCoord == Coord.Invalid) 
        StartCoord = tilePresenter.FindClosestCoord(unit.transform.position, unit.Player);
    }
    
    void EndDrag() {
      if (battleStateController.IsBattleStarted) return;
      var selectedPlayerId = battleSetupUI.GetSelectedPlayerId;
      if (unit.Player != (EPlayer)selectedPlayerId) return;
      
      isDragging = false;
      tilePresenter.TileAt(lastCoord).Unhighlight();
      
      var player = players[selectedPlayerId];
      player.MoveUnit(StartCoord, lastCoord);
      
      var playerPresenter = playerPresenters[selectedPlayerId];
      playerPresenter.MoveUnit(StartCoord, lastCoord);
      
      lastCoord = Coord.Invalid;
    }
    
    public void Drag() {
      if (battleStateController.IsBattleStarted) return;
      if (!isDragging) return;

      var plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
      var ray = cam.ScreenPointToRay(Input.mousePosition);

      if (!plane.Raycast(ray, out var enter)) return;

      var mousePosition = ray.GetPoint(enter);
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

    RaycastController raycastController;
    Camera cam;
    bool isDragging;
    public Coord StartCoord;
    Coord lastCoord = Coord.Invalid;
    TilePresenter tilePresenter;
    BattleSetupUI battleSetupUI;
    Player[] players;
    PlayerPresenter[] playerPresenters;
    UnitTooltipController unitTooltipController;
    BattleStateController battleStateController;
    UnitView unit;
  }
}