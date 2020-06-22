using PlasticFloor.EventBus;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class IdleEvent : IEvent {
    public Coord Coord;
    
    public IdleEvent(Coord coord) => Coord = coord;

    public override string ToString() => $"{nameof(Coord)}: {Coord}";
  }
}