using System.Collections;
using System.Collections.Generic;
using Shared;
using UnityEngine;
using View.Exts;
using static UnityEngine.Mathf;

namespace View {
  public class BoardView : MonoBehaviour, IUnitHolder {
    [SerializeField] Transform StartPoint;

    public EUnitHolder Type { get; } = EUnitHolder.Board;

    void Start() {
      for (int x = 0; x < 8; x++) {
        for (int y = 0; y < 6; y++) {
          var coord = new Coord(x, y);
          tiles[coord] = tileFactory.Create(StartPoint.position + new Vector3(x, 0, y), 
            x, y, this);
        }
      }
    }  
    
    public void Init(TileViewFactory tileFactory, IUnitViewFactory unitFactory) {
      this.tileFactory = tileFactory;
      this.unitFactory = unitFactory;
    }
    
    public TileView FindClosestTile(Vector3 position, EPlayer selectedPlayer) {
      var indexPosition = position - StartPoint.position;
      var indexX = RoundToInt(indexPosition.x);
      var indexY = RoundToInt(indexPosition.z);
      var x = Clamp(indexX, 0, 7);
      var y = selectedPlayer == EPlayer.First
        ? Clamp(indexY, 0, 2)
        : Clamp(indexY, 3, 5);
      var coord = new Coord(x, y);

      return tiles[coord];
    }
    
    Vector3 TilePosition(Coord coord) => StartPoint.position + new Vector3(coord.X, 0, coord.Y);

    public void Place(UnitView unit, TileView tile) {
      unit.transform.position = TilePosition(new Coord(tile.X, tile.Y)).WithY(unit.Height);
      units[new Coord(tile.X, tile.Y)] = unit;
    }
    
    public void Unplace(UnitView unit, TileView tile) {
      units.Remove(new Coord(tile.X, tile.Y));
    }

    public void Clear() {
      foreach (var unit in units.Values) {
        Destroy(unit.gameObject);
      }
      units.Clear();
    }

    public void AddUnit(string name, Coord coord, EPlayer player) {
      var position = TilePosition(coord);
      var tile = tiles[coord];
      units[coord] = unitFactory.Create(name, position, tile, player);
    }

    public void Move(Coord from, Coord to) {
      var fromUnit = units[from];
      fromUnit.transform.position = TilePosition(to).WithY(fromUnit.Height);
      units[to] = fromUnit;
      units.Remove(from);
    }

    public MoveRoutine MoveRoutine(Coord from, Coord to, float startingTime, float time) => 
      new MoveRoutine(units[from], tiles[from], tiles[to], startingTime, time);

    readonly Dictionary<Coord, TileView> tiles = new Dictionary<Coord, TileView>(8 * 6);
    readonly Dictionary<Coord, UnitView> units = new Dictionary<Coord, UnitView>(10);
    TileViewFactory tileFactory;
    IUnitViewFactory unitFactory;
  }
}