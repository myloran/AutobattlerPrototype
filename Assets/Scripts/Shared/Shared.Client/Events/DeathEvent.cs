using PlasticFloor.EventBus;
using Shared.Poco;

namespace Shared.Shared.Client.Events {
  public class DeathEvent : IEvent {
    public Coord Coord { get; }

    public DeathEvent(Coord coord) {
      Coord = coord;
    }
    
    public override string ToString() => $"{nameof(Coord)}: {Coord}";
  }
}