using Model.NUnit.Abstraction;
using Shared;
using Shared.Abstraction;

namespace Model.NUnit {
  public class UnitCoordChangedHandler : IHandler<UnitCoordChanged<IUnit>> {
    public void Handle(UnitCoordChanged<IUnit> e) => e.Unit.StartingCoord = e.To;
  }
}