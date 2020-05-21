using UnityEngine;

namespace View {
  public interface IUnitViewFactory {
    UnitView Create(string name, Vector3 position, TileView tile, EPlayer player);
  }
}