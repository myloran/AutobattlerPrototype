using PlasticFloor.EventBus;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class RotateEvent : IEvent {
    public Coord From, To;

    public RotateEvent(Coord @from, Coord to) {
      From = @from;
      To = to;
    }

    public override string ToString() => $"{nameof(From)}: {From}, {nameof(To)}: {To}";
  }
}