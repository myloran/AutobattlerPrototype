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
          tiles[x, y] = tileFactory.Create(StartPoint.position + new Vector3(x, 0, y),
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
      var z = selectedPlayer == EPlayer.First
        ? Clamp(indexY, 0, 2)
        : Clamp(indexY, 3, 5);

      return tiles[x, z];
    }
    
    Vector3 TilePosition(int x, int z) => StartPoint.position + new Vector3(x, 0, z);

    public void Place(UnitView unit, TileView tile) {
      unit.transform.position = TilePosition(tile.X, tile.Y).WithY(unit.Height);
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
      var position = TilePosition(coord.X, coord.Y);
      var tile = tiles[coord.X, coord.Y];
      units[coord] = unitFactory.Create(name, position, tile, player);
    }

    public void Move(Coord from, Coord to, float time) {
      var fromUnit = units[from];
      var fromTile = tiles[from.X, from.Y];
      var toTile = tiles[to.X, to.Y];
      var moveRoutine = new MoveRoutine(fromUnit, fromTile, toTile, time, this);
    }
    
    readonly TileView[,] tiles = new TileView[8, 6];
    readonly Dictionary<Coord, UnitView> units = new Dictionary<Coord, UnitView>(10);
    TileViewFactory tileFactory;
    IUnitViewFactory unitFactory;
  }
}