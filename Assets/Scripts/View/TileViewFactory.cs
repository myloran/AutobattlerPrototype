using UnityEngine;

namespace View {
  public class TileViewFactory {
    public TileViewFactory(TileView tilePrefab) {
      this.tilePrefab = tilePrefab;
    }

    public TileView Create(Vector3 position) =>
      Object.Instantiate(tilePrefab, position, Quaternion.Euler(90, 0, 0));

    readonly TileView tilePrefab;
  }
}