using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Abstraction;

namespace Model {
  public class UnitCoordChangedHandler : IHandler<UnitCoordChanged<IUnit>> {
    public void Handle(UnitCoordChanged<IUnit> e) => e.Unit.StartingCoord = e.To;
  }
}