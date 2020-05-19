using Shared;
using UnityEngine;

namespace View {
  public class BenchView : MonoBehaviour, IUnitHolder {
    [SerializeField] Transform StartPoint;
    public EUnitHolder Type { get; } = EUnitHolder.Bench;
    
    void Start() {
      for (int x = 0; x < 10; x++) {
        tiles[x] = tileFactory.Create(TilePosition(x), x, -1, this);
      }
    }

    public void Init(IUnitViewFactory unitFactory, TileViewFactory tileFactory) {
      this.unitFactory = unitFactory;
      this.tileFactory = tileFactory;
    }

    public float StartZPosition => StartPoint.position.z;

    public (bool, Coord) AddUnit(string name) {
      for (int x = 0; x < 10; x++) {
        if (units[x] != null) continue;

        units[x] = unitFactory.Create(name, TilePosition(x), tiles[x]);
        return (true, new Coord(x, -1));
      }

      return (false, default);
    }
    
    public (bool, Coord) RemoveUnit() {
      for (int x = 9; x >= 0; x--) {
        if (units[x] == null) continue;
        
        Destroy(units[x].gameObject);
        units[x] = null;
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
    }
    
    readonly TileView[] tiles = new TileView[10];
    readonly UnitView[] units = new UnitView[10];
    IUnitViewFactory unitFactory;
    TileViewFactory tileFactory;
  }
}