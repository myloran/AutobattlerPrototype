using System.Collections.Generic;
using Shared;
using UnityEngine;
using View.Exts;
using static UnityEngine.Mathf;

namespace View {
  public class BoardView : IUnitHolder {
    Transform startPoint;
    public readonly Dictionary<Coord, UnitView> Units = new Dictionary<Coord, UnitView>(10);
    public EUnitHolder Type { get; } = EUnitHolder.Board;
    
    public BoardView() { }

    public void Init(Transform startPoint, TileViewFactory tileFactory, IUnitViewFactory unitFactory) {
      this.startPoint = startPoint;
      this.unitFactory = unitFactory;
      
      for (int x = 0; x < 8; x++) {
        for (int y = 0; y < 6; y++) {
          var coord = new Coord(x, y);
          tiles[coord] = tileFactory.Create(startPoint.position + new Vector3(x, 0, y), 
            x, y, this);
        }
      }
    }
    
    public TileView FindClosestTile(Vector3 position, EPlayer selectedPlayer) {
      var indexPosition = position - startPoint.position;
      var indexX = RoundToInt(indexPosition.x);
      var indexY = RoundToInt(indexPosition.z);
      var x = Clamp(indexX, 0, 7);
      var y = selectedPlayer == EPlayer.First
        ? Clamp(indexY, 0, 2)
        : Clamp(indexY, 3, 5);
      var coord = new Coord(x, y);

      return tiles[coord];
    }
    
    public Vector3 TilePosition(Coord coord) => startPoint.position + new Vector3(coord.X, 0, coord.Y);

    public void Place(UnitView unit, TileView tile) {
      unit.transform.position = TilePosition(new Coord(tile.X, tile.Y)).WithY(unit.Height);
      Units[new Coord(tile.X, tile.Y)] = unit;
    }
    
    public void Unplace(UnitView unit, TileView tile) {
      Units.Remove(new Coord(tile.X, tile.Y));
    }

    public void Clear() {
      foreach (var unit in Units.Values) {
        Object.Destroy(unit.gameObject);
      }
      Units.Clear();
    }

    public void AddUnit(string name, Coord coord, EPlayer player) {
      var position = TilePosition(coord);
      var tile = tiles[coord];
      Units[coord] = unitFactory.Create(name, position, tile, player);
    }

    public void Move(Coord from, Coord to) {
      var fromUnit = Units[from];
      fromUnit.transform.position = TilePosition(to).WithY(fromUnit.Height);
      Units[to] = fromUnit;
      Units.Remove(from);
    }


    public MoveRoutine MoveRoutine(Coord from, Coord to, float startingTime, float time) => 
      new MoveRoutine(Units[from], tiles[from], tiles[to], startingTime, time);

    readonly Dictionary<Coord, TileView> tiles = new Dictionary<Coord, TileView>(8 * 6);
    IUnitViewFactory unitFactory;
  }
}