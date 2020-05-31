using PlasticFloor.EventBus;

namespace Shared.Shared.Client.Events {
  public class ApplyDamageEvent : IEvent {
    public float Health { get; }
    public Coord Coord { get; }

    public ApplyDamageEvent(float health, Coord coord) {
      Health = health;
      Coord = coord;
    }
  }
}