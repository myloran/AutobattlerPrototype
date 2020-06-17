using PlasticFloor.EventBus;

namespace Shared.Shared.Client.Events {
  public class StartAttackEvent : IEvent {
    public Coord Coord;

    public StartAttackEvent(Coord coord) => Coord = coord;

    public override string ToString() => $"{nameof(Coord)}: {Coord}";
  }
}