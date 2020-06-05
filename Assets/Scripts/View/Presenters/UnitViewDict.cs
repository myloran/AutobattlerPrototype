using Shared;
using UnityEngine;

namespace View.Presenters {
  public class UnitViewDict : BaseUnitDict<UnitView> {
    public UnitViewDict(IUnitViewFactory unitFactory) => this.unitFactory = unitFactory;

    public override UnitView this[Coord coord] {
      get => base[coord];
      set {
        base[coord] = value;
        
      }
    }

    protected override UnitView Create(string name, Coord coord, EPlayer player) => 
      unitFactory.Create(name, coord, player);

    protected override void Remove(UnitView unit) => Object.Destroy(unit.gameObject);

    readonly IUnitViewFactory unitFactory;
  }
}