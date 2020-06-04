using System.Collections.Generic;
using Shared;
using UnityEngine;
using View.Exts;

namespace View {
  public class BenchView : IUnitHolder {
    public EPlayer Player { get; private set; }
    public EUnitHolder Type { get; } = EUnitHolder.Bench;
    
    public BenchView() {}
    
    public void Init(IUnitViewFactory unitFactory, TileViewFactory tileFactory, EPlayer player, Transform startPoint) {
      this.unitFactory = unitFactory;
      this.Player = player;
      this.startPoint = startPoint;

      for (int x = 0; x < 10; x++) {
        var coord = new Coord(x, -1);
        tiles[coord] = tileFactory.Create(TilePosition(x), x, -1, this);
      }
    }

    public float StartZPosition => startPoint.position.z;

    public (bool, Coord) AddUnit(string name) {
      for (int x = 0; x < 10; x++) {
        var coord = new Coord(x, -1);
        if (units.ContainsKey(coord)) continue;

        units[coord] = unitFactory.Create(name, TilePosition(x), tiles[coord], Player);
        return (true, new Coord(x, -1));
      }

      return (false, default);
    }
    
    public void AddUnit(string name, Coord coord) => 
      units[coord] = unitFactory.Create(name, TilePosition(coord.X), tiles[coord], Player);

    public (bool, Coord) RemoveUnit() {
      for (int x = 9; x >= 0; x--) {
        var coord = new Coord(x, -1);
        if (!units.ContainsKey(coord)) continue;
        
        Object.Destroy(units[coord].gameObject);
        units.Remove(coord);
        return (true, new Coord(x, -1));
      }
      return (false, default);
    }
    
    Vector3 TilePosition(int x) => startPoint.position + new Vector3(x, 0, 0);
    
    public TileView FindClosestTile(Vector3 position) {
      var indexPosition = position - startPoint.position;
      var index = Mathf.RoundToInt(indexPosition.x);
      var indexClamped = Mathf.Clamp(index, 0, 9);  
      var coord = new Coord(indexClamped, -1);
      
      return tiles[coord];
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
        Object.Destroy(unit.gameObject);
      }
      units.Clear();
    }

    readonly Dictionary<Coord, TileView> tiles = new Dictionary<Coord, TileView>(10);
    readonly Dictionary<Coord, UnitView> units = new Dictionary<Coord, UnitView>(10);
    IUnitViewFactory unitFactory;
    Transform startPoint;
  }
}