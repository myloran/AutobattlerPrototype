using Controller.NBattleSimulation;
using Model.NBattleSimulation;
using Shared;
using UnityEngine;
using View;
using View.Presenters;
using View.UI;

namespace Controller {
  //show tooltip, show unit model, dragging, player validation, move/swap unit,
  //tile highlight, find tile
  public class UnitDragController : MonoBehaviour {
    public void Init(TilePresenter tilePresenter, BattleSetupUI battleSetupUI,
        Player[] players, PlayerPresenter[] playerPresenters, UnitTooltipController unitTooltipController, UnitView unit,
        BattleStateController battleStateController) {
      this.tilePresenter = tilePresenter;
      this.battleSetupUI = battleSetupUI;
      this.players = players;
      this.playerPresenters = playerPresenters;
      this.unitTooltipController = unitTooltipController;
      this.unit = unit;
      this.battleStateController = battleStateController;
      
      StartCoord = tilePresenter.FindClosestCoord(transform.position, unit.Player);
    }

    void Awake() { //TODO: remove
      cam = Camera.main;
    }

    void OnMouseDown() {
      if (battleStateController.IsBattleStarted) {
        // if (isDebug)
        //   unitDebugController.Show();
        // else
          unitTooltipController.Show(unit.Info);
        return;
      }
      if (unit.Player != (EPlayer)battleSetupUI.GetSelectedPlayerId) return;
      
      isDragging = true;
    }

    void OnMouseUp() {
      if (battleStateController.IsBattleStarted) return;
      var selectedPlayerId = battleSetupUI.GetSelectedPlayerId;
      if (unit.Player != (EPlayer)selectedPlayerId) return;
      
      isDragging = false;
      tilePresenter.TileAt(lastCoord).Unhighlight();
      
      var player = players[selectedPlayerId];
      player.MoveUnit(StartCoord, lastCoord);
      
      var playerPresenter = playerPresenters[selectedPlayerId];
      playerPresenter.MoveUnit(StartCoord, lastCoord);

      var dict = StartCoord.Y < 0 ? playerPresenter.BenchUnits : playerPresenter.BoardUnits;
      if(dict.Contains(StartCoord)) {
        dict[StartCoord].GetComponent<UnitDragController>().StartCoord = StartCoord;
      }
      
      StartCoord = lastCoord;
    }

    void Update() {
      if (battleStateController.IsBattleStarted) return;
      if (!isDragging) return;

      var plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
      var ray = cam.ScreenPointToRay(Input.mousePosition);

      if (!plane.Raycast(ray, out var enter)) return;
      
      var mousePosition = ray.GetPoint(enter);
      transform.position = mousePosition + new Vector3(0, 1, 0);

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

    Camera cam;
    bool isDragging;
    UnitView unit;
    public Coord StartCoord;
    Coord lastCoord = Coord.Invalid;
    TilePresenter tilePresenter;
    BattleSetupUI battleSetupUI;
    Player[] players;
    PlayerPresenter[] playerPresenters;
    UnitTooltipController unitTooltipController;
    BattleStateController battleStateController;
  }
}