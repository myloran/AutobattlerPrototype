using UnityEngine;

namespace View {
  public class BenchView : MonoBehaviour {
    public TileView TilePrebab;
    public Transform StartPoint;
    public UnitView UnitPrefab;
    
    void Start() {
      for (int x = 0; x < 10; x++) {
        tiles[x] = Instantiate(TilePrebab, TilePosition(x), Quaternion.Euler(90, 0, 0));
        tiles[x].X = x;
        tiles[x].Y = -1;
      }
    }

    public void AddUnit() {
      for (int x = 0; x < 10; x++) {
        if (units[x] != null) continue;
        
        units[x] = Instantiate(UnitPrefab, TilePosition(x), Quaternion.identity);
        break;
      }
    }
    
    Vector3 TilePosition(int x) => StartPoint.position + new Vector3(x, 0, 0);

    readonly TileView[] tiles = new TileView[10];
    readonly UnitView[] units = new UnitView[10];
  }
}