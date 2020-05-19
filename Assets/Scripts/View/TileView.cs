using UnityEngine;
using View;

public class TileView : MonoBehaviour {
  public UnitView Unit;
  public IUnitHolder Holder;
  public int X, Y;

  void Start() {
    var meshRenderer = GetComponent<MeshRenderer>();
    meshRenderer.material = material = new Material(meshRenderer.material);
  }
  
  public TileView Init(IUnitHolder unitHolder, int x, int y) {
    Holder = unitHolder;
    X = x;
    Y = y;
    return this;
  }
  
  public void PlaceUnit(UnitView unit) {
    Holder.Place(unit, this);
    Unit = unit;
    unit.Tile = this;
  }

  public void Highlight() => material.color = Color.green;
  public void Unhighlight() => material.color = Color.white;
  
  Material material;
}
