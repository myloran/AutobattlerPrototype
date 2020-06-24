using PlasticFloor.EventBus;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class FinishMoveEvent : IEvent {
    public Coord From { get; }
    public Coord To { get; }

    public FinishMoveEvent(Coord from, Coord to) {
      From = @from;
      To = to;
    }
    
    public override string ToString() => $"{nameof(From)}: {From}, {nameof(To)}: {To}";
  }
}