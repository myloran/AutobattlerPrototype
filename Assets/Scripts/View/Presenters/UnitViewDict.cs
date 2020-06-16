using Shared;
using Shared.Abstraction;
using UnityEngine;
using View.Factories;
using View.Views;

namespace View.Presenters {
  public class UnitViewDict : BaseUnitDict<UnitView> {
    public UnitViewDict(UnitViewFactory unitFactory) => this.unitFactory = unitFactory;
    
    protected override UnitView Create(string name, Coord coord, EPlayer player) => 
      unitFactory.Create(name, coord, player);

    protected override void Remove(UnitView unit) => Object.Destroy(unit.gameObject);

    readonly UnitViewFactory unitFactory;
  }
}