using FixMath;
using PlasticFloor.EventBus;

namespace Shared.Shared.Client.Events {
  public class StartMoveEvent : IEvent {
    public Coord From { get; }
    public Coord To { get; }
    public F32 StartingTime { get; }
    public F32 Duration { get; }

    public StartMoveEvent(Coord @from, Coord to, F32 startingTime, F32 duration) {
      From = @from;
      To = to;
      StartingTime = startingTime;
      Duration = duration;
    }
    
    public override string ToString() => $"{nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(StartingTime)}: {StartingTime}, {nameof(Duration)}: {Duration}";
  }
}