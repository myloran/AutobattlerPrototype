using PlasticFloor.EventBus;

namespace Shared.Shared.Client.Events {
  public class StartMoveEvent : IEvent {
    public Coord From { get; }
    public Coord To { get; }
    public TimePoint StartingTime { get; }
    public float Duration { get; }

    public StartMoveEvent(Coord @from, Coord to, TimePoint startingTime, float duration) {
      From = @from;
      To = to;
      StartingTime = startingTime;
      Duration = duration;
    }
    
    public override string ToString() => $"{nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(StartingTime)}: {StartingTime}, {nameof(Duration)}: {Duration}";
  }
}