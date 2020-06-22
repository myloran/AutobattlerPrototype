using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class UpdateHealthEvent : IEvent {
    public F32 Health { get; } 
    public Coord Coord { get; }

    public UpdateHealthEvent(F32 health, Coord coord) {
      Health = health;
      Coord = coord;
    }

    public override string ToString() => $"{nameof(Health)}: {Health}, {nameof(Coord)}: {Coord}";
  }
}