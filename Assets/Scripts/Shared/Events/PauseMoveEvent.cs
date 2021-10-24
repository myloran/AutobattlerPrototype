using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class PauseMoveEvent : IEvent {
    public F32 PauseDuration { get; } 
    public Coord Coord { get; }

    public PauseMoveEvent(F32 pauseDuration, Coord coord) {
      PauseDuration = pauseDuration;
      Coord = coord;
    }

    public override string ToString() => $"{nameof(PauseDuration)}: {PauseDuration}, {nameof(Coord)}: {Coord}";
  }
}