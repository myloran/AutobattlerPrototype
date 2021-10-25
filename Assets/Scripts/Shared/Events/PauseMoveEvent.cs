using PlasticFloor.EventBus;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class PauseMoveEvent : IEvent {
    public Coord Coord { get; }

    public PauseMoveEvent(Coord coord) {
      Coord = coord;
    }

    public override string ToString() => $"{nameof(Coord)}: {Coord}";
  }
}