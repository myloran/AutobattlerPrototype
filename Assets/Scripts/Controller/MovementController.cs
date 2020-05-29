using PlasticFloor.EventBus;
using Shared.Events;

namespace Controller {
  public class MovementController : IEventHandler<StartMoveEvent> {
    public void HandleEvent(StartMoveEvent e) {
      throw new System.NotImplementedException();
    }
  }
}