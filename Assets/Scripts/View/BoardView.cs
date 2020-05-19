using UnityEngine;

namespace View {
  public class BoardView : MonoBehaviour, IUnitHolder {
    [SerializeField] Transform StartPoint;
    public EUnitHolder Type { get; } = EUnitHolder.Board;

    void Start() {
      for (int x = 0; x < 8; x++) {
        for (int y = 0; y < 6; y++) {
          tiles[x, y] = tileViewFactory.Create(StartPoint.position + new Vector3(x, 0, y),
            x, y, this);
        }
      }
    }  
    
    public void Init(TileViewFactory tileViewFactory) {
      this.tileViewFactory = tileViewFactory;
    }
    
    public TileView FindClosestTile(Vector3 position) {
      var indexPosition = position - StartPoint.position;
      var indexX = Mathf.RoundToInt(indexPosition.x);
      var indexY = Mathf.RoundToInt(indexPosition.z);
      var x = Mathf.Clamp(indexX, 0, 7);
      var z = Mathf.Clamp(indexY, 0, 5);

      return tiles[x, z];
    }
    
    Vector3 TilePosition(int x, int z) => StartPoint.position + new Vector3(x, 0, z);

    public void Place(UnitView unit, TileView tile) {
      unit.transform.position = TilePosition(tile.X, tile.Y).WithY(unit.Height);
    }
    
    readonly TileView[,] tiles = new TileView[8, 6];
    TileViewFactory tileViewFactory;
  }
}