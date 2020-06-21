using System.Collections.Generic;
using Shared;
using Shared.Poco;
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
    public UnitView GetUnit(Coord coord) => units[coord];

    #endregion

    public void MoveUnit(Coord from, Coord to) {
      var unit = units[from];
      handler.Handle(new UnitCoordChanged<UnitView>(unit, to));
      AddUnit(to, unit);
      RemoveUnit(from);
    }
    
    public void Reset(Dictionary<Coord, UnitView> units) => this.units = units;

    readonly UnitViewCoordChangedHandler handler;
    Dictionary<Coord, UnitView> units = new Dictionary<Coord, UnitView>();
  }
}