using Model.NUnit;
using Shared;
using Shared.Abstraction;

namespace Model {
  public class UnitDict : BaseUnitDict<Unit> {
    public UnitDict(UnitFactory unitFactory) => this.unitFactory = unitFactory;
    
    protected override Unit Create(string name, Coord coord, EPlayer player) => 
      unitFactory.Create(name, coord, player);

    readonly UnitFactory unitFactory;
  }
}