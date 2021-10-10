using System.Collections.Generic;
using Shared;
using Shared.Primitives;
using View.Exts;
using View.NTile;
using View.NUnit;

namespace View.Presenters {
  public class BoardPresenter {
    public BoardPresenter(UnitViewCoordChangedHandler handler) => this.handler = handler;

    #region Dict

    public IEnumerable<UnitView> Values => units.Values;
    public void AddUnit(Coord coord, UnitView unit) => units[coord] = unit;
    public void RemoveUnit(Coord coord) => units.Remove(coord);
    public bool ContainsUnit(Coord coord) => units.ContainsKey(coord); 
    public UnitView TryGetUnit(Coord coord) => units.TryGetValue(coord, out var unit) ? unit : null;

    #endregion

    public void MoveUnit(Coord from, Coord to) {
      if (!units.TryGetValue(from, out var unit)) return;
      
      handler.Handle(new UnitCoordChanged<UnitView>(unit, to));
      AddUnit(to, unit);
      RemoveUnit(from);
    }
    
    public void SetUnits(Dictionary<Coord, UnitView> units) => this.units = units;

    readonly UnitViewCoordChangedHandler handler;
    Dictionary<Coord, UnitView> units = new Dictionary<Coord, UnitView>();
  }
}