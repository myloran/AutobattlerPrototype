using Model;
using Model.NBattleSimulation;
using Shared;
using UnityEngine;
using View;
using View.UI;

namespace Controller {
  public class UnitDragController : MonoBehaviour {
    public void Init(ClosestTileFinder closestTileFinder, BattleSetupUI battleSetupUI, 
        Player[] players, UnitView unit) {
      this.closestTileFinder = closestTileFinder;
      this.battleSetupUI = battleSetupUI;
      this.players = players;
      this.unit = unit;
    }

    void Awake() => cam = Camera.main;
    
    void OnMouseDown() {
      if (unit.Player != (EPlayer)battleSetupUI.GetSelectedPlayerId) return;
      
      dragging = true;
    }

    void OnMouseUp() {
      if (unit.Player != (EPlayer)battleSetupUI.GetSelectedPlayerId) return;
      
      dragging = false;
      lastTile?.Unhighlight();
        
      var player = players[battleSetupUI.GetSelectedPlayerId];
      var from = new Coord(unit.Tile.X, unit.Tile.Y);
      var to = new Coord(lastTile.X, lastTile.Y);
      
      player.MoveUnit(from, to);
      
      if (lastTile?.Unit != null) {
        unit.SwapWith(lastTile.Unit);
      }
      else {
        unit.MoveTo(lastTile);
      }
    }

    void Update() {
      if (!dragging) return;

      var plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
      var ray = cam.ScreenPointToRay(Input.mousePosition);

      if (!plane.Raycast(ray, out var enter)) return;
      
      var mousePosition = ray.GetPoint(enter);
      transform.position = mousePosition + new Vector3(0, 1, 0);

      var tile = closestTileFinder.Find(mousePosition, (EPlayer)battleSetupUI.GetSelectedPlayerId);
      
      if (IsNewTile(tile)) {
        lastTile?.Unhighlight();
        tile.Highlight();
        //   //if swap previous, cancel it
        //   if (tile.Unit != null) {
        //     SwapUnits(tile, oldTile);
        //   }
        //   //swap units if tile with unit
      }

      lastTile = tile;
    }

    bool IsNewTile(TileView tile) => tile.X != lastTile?.X || tile.Y != lastTile?.Y;

    Camera cam;
    bool dragging;
    UnitView unit;
    TileView lastTile;
    ClosestTileFinder closestTileFinder;
    BattleSetupUI battleSetupUI;
    Player[] players;
  }
}