using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class UpdateStunDurationEvent : IEvent {
    public F32 Duration { get; } 
    public Coord Coord { get; }

    public UpdateStunDurationEvent(F32 duration, Coord coord) {
      Duration = duration;
      Coord = coord;
    }

    public override string ToString() => $"{nameof(Duration)}: {Duration}, {nameof(Coord)}: {Coord}";
  }
}