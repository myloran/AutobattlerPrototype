using UnityEngine;

namespace View {
  public class BenchView : MonoBehaviour {
    public TileView TilePrebab;
    public Transform StartPoint;
    
    void Start() {
      for (int x = 0; x < 10; x++) {
        tiles[x] = Instantiate(TilePrebab, TilePosition(x), Quaternion.Euler(90, 0, 0));
        tiles[x].X = x;
        tiles[x].Y = -1;
      }
    }

    public void Init(UnitViewFactory factory) {
      unitFactory = factory;
    }

    public void AddUnit(string name) {
      for (int x = 0; x < 10; x++) {
        if (units[x] != null) continue;

        units[x] = unitFactory.Create(name, TilePosition(x));
        break;
      }
    }
    
    Vector3 TilePosition(int x) => StartPoint.position + new Vector3(x, 0, 0);

    readonly TileView[] tiles = new TileView[10];
    readonly UnitView[] units = new UnitView[10];
    UnitViewFactory unitFactory;
  }
}