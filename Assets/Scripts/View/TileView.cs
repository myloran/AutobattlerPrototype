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

  public void MoveUnitHere(UnitView fromUnit) {
    var fromTile = fromUnit.Tile;
    fromTile.UnplaceUnitFromHolder();
    fromTile.UnplaceUnitFromTile();
    PlaceUnitToTile(fromUnit);
    PlaceUnitToHolder();
  }

  public void SwapUnits(UnitView fromUnit) {
    var fromTile = fromUnit.Tile;
    fromTile.UnplaceUnitFromHolder();
    UnplaceUnitFromHolder();
    fromTile.PlaceUnitToTile(Unit);
    PlaceUnitToTile(fromUnit);
    fromTile.PlaceUnitToHolder();
    PlaceUnitToHolder();
  }

  public void PlaceUnitToHolder() => Holder.Place(Unit, this);
  public void UnplaceUnitFromHolder() => Holder.Unplace(Unit, this);

  public void PlaceUnitToTile(UnitView unit) {
    unit.Tile = this;
    Unit = unit;
  }

  public void UnplaceUnitFromTile() {
    if (Unit == null) return;
    
    Unit.Tile = null;
    Unit = null;
  }

  public void Highlight() => material.color = Color.green;
  public void Unhighlight() => material.color = Color.white;
  
  Material material;
}
