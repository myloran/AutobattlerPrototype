using System.Collections.Generic;
using Shared;
using UnityEngine;
using View.Exts;

namespace View {
  public class BenchView : MonoBehaviour, IUnitHolder {
    [SerializeField] Transform StartPoint;
    public EPlayer Player { get; private set; }

    public EUnitHolder Type { get; } = EUnitHolder.Bench;
    
    void Start() {
      for (int x = 0; x < 10; x++) {
        tiles[x] = tileFactory.Create(TilePosition(x), x, -1, this);
      }
    }

    public void Init(IUnitViewFactory unitFactory, TileViewFactory tileFactory, EPlayer player) {
      this.unitFactory = unitFactory;
      this.tileFactory = tileFactory;
      this.Player = player;
    }

    public float StartZPosition => StartPoint.position.z;

    public (bool, Coord) AddUnit(string name) {
      for (int x = 0; x < 10; x++) {
        var coord = new Coord(x, -1);
        if (units.ContainsKey(coord)) continue;

        units[coord] = unitFactory.Create(name, TilePosition(x), tiles[x], Player);
        return (true, new Coord(x, -1));
      }

      return (false, default);
    }
    
    public void AddUnit(string name, Coord coord) => 
      units[coord] = unitFactory.Create(name, TilePosition(coord.X), tiles[coord.X], Player);

    public (bool, Coord) RemoveUnit() {
      for (int x = 9; x >= 0; x--) {
        var coord = new Coord(x, -1);
        if (!units.ContainsKey(coord)) continue;
        
        Destroy(units[coord].gameObject);
        units.Remove(coord);
        return (true, new Coord(x, -1));
      }
      return (false, default);
    }
    
    Vector3 TilePosition(int x) => StartPoint.position + new Vector3(x, 0, 0);
    
    public TileView FindClosestTile(Vector3 position) {
      var indexPosition = position - StartPoint.position;
      var index = Mathf.RoundToInt(indexPosition.x);
      var indexClamped = Mathf.Clamp(index, 0, 9);
      
      return tiles[indexClamped];
    }
    
    public void Place(UnitView unit, TileView tile) {
      unit.transform.position = TilePosition(tile.X).WithY(unit.Height);
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

    readonly TileView[] tiles = new TileView[10];
    readonly Dictionary<Coord, UnitView> units = new Dictionary<Coord, UnitView>(10);
    IUnitViewFactory unitFactory;
    TileViewFactory tileFactory;
  }
}