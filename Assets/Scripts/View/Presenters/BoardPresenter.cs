using System.Collections.Generic;
using Shared;
using View.Exts;
using View.NUnit;

namespace View.Presenters {
  public class BoardPresenter {
    public BoardPresenter(TilePresenter tilePresenter) => this.tilePresenter = tilePresenter;

    #region Dict

    public IEnumerable<UnitView> Values => units.Values;
    public void AddUnit(Coord coord, UnitView unit) => units[coord] = unit;
    public void RemoveUnit(Coord coord) => units.Remove(coord);
    public bool ContainsUnit(Coord coord) => units.ContainsKey(coord); 
    public UnitView GetUnit(Coord coord) => units[coord];

    #endregion

    public void MoveUnit(Coord from, Coord to) {
      var unit = units[from];
      OnChangeCoord(to, unit);
      AddUnit(to, unit);
      RemoveUnit(from);
    }
    
    public Dictionary<Coord, UnitView> Reset(PlayerPresenter player1, PlayerPresenter player2) {
      units.Clear();
      
      foreach (var (coord, unit) in player1.BoardUnits) units[coord] = unit;
      foreach (var (coord, unit) in player2.BoardUnits) units[coord] = unit;
      
      return units;
    }
    
    void OnChangeCoord(Coord coord, UnitView unit) {
      var toPosition = tilePresenter.PositionAt(coord).WithY(unit.Height);
      unit.transform.position = toPosition;
    }

    readonly TilePresenter tilePresenter;
    readonly Dictionary<Coord, UnitView> units = new Dictionary<Coord, UnitView>();
  }
}