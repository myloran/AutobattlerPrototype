using Model.NUnit;
using Shared;
using Shared.Abstraction;

namespace Model {
  public class UnitCoordChangedHandler : IHandler<UnitCoordChanged<Unit>> {
    public void Handle(UnitCoordChanged<Unit> e) => e.Unit.StartingCoord = e.To;
  }
}