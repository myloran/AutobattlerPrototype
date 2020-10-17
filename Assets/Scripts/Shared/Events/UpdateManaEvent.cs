using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;

namespace Shared.Shared.Client.Events {
  public class UpdateManaEvent : IEvent {
    public F32 Mana { get; } 
    public Coord Coord { get; }

    public UpdateManaEvent(F32 mana, Coord coord) {
      Mana = mana;
      Coord = coord;
    }

    public override string ToString() => $"{nameof(Mana)}: {Mana}, {nameof(Coord)}: {Coord}";
  }
}