using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class ContinueMoveEvent : IEvent {
    public Coord Coord { get; }
    public F32 StartingTime { get; }
    public F32 Duration { get; }

    public ContinueMoveEvent(Coord coord, F32 startingTime, F32 duration) {
      Coord = coord;
      StartingTime = startingTime;
      Duration = duration;
    }
    
    public override string ToString() => $"{nameof(Coord)}: {Coord}, {nameof(StartingTime)}: {StartingTime}, {nameof(Duration)}: {Duration}";
  }
}