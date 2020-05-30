using Shared;
using UnityEngine;

namespace View {
  public class UnitView : MonoBehaviour {
    public TileView Tile;
    public UnitInfo Info;
    public float Height = 0.25f;
    public EPlayer Player;
    public int Level = 1;

    public void SwapWith(UnitView unit) => Tile.SwapUnits(unit);
    public void MoveTo(TileView tile) => tile.MoveUnitHere(this);
  }
}