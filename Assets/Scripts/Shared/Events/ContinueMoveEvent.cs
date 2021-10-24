using PlasticFloor.EventBus;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class ContinueMoveEvent : IEvent {
    public Coord Coord { get; }

    public ContinueMoveEvent(Coord coord) {
      Coord = coord;
    }
    
    public override string ToString() => $"{nameof(Coord)}: {Coord}";
  }
}