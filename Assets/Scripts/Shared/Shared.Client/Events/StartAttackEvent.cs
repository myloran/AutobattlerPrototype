using PlasticFloor.EventBus;
using Shared.Poco;

namespace Shared.Shared.Client.Events {
  public class StartAttackEvent : IEvent { //TODO: rename to SwitchToAttackStateEvent
    public Coord Coord;

    public StartAttackEvent(Coord coord) => Coord = coord;

    public override string ToString() => $"{nameof(Coord)}: {Coord}";
  }
}