using UnityEngine;

namespace View {
  public class TileViewFactory {
    public TileViewFactory(TileView tilePrefab) {
      this.tilePrefab = tilePrefab;
    }

    public TileView Create(Vector3 position, int x, int y, IUnitHolder unitHolder) {
      return Object.Instantiate(tilePrefab, position, Quaternion.Euler(90, 0, 0))
        .Init(unitHolder, x, y);
    }

    readonly TileView tilePrefab;
  }
}