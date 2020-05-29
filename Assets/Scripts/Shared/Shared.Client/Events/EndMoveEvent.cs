using PlasticFloor.EventBus;

namespace Shared.Shared.Client.Events {
  public class EndMoveEvent : IEvent {
    public Coord From { get; }
    public Coord To { get; }

    public EndMoveEvent(Coord from, Coord to) {
      From = @from;
      To = to;
    }
  }
}