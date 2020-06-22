using PlasticFloor.EventBus;
using Shared.Addons.Examples.FixMath;
using Shared.Shared.Client.Abstraction;
using static Shared.Addons.Examples.FixMath.F32;

namespace Shared.Shared.Client {
  public class EventHolder : IEventBus { //TODO: implement easy replay by resetting/replaying events to selected time on high time scale?
    public bool NeedExecuteImmediately = true;
      
    public EventHolder(IEventBus bus) {
      this.bus = bus;
    }
    
    public void SetTime(ITime time) => this.time = time;

    public bool HasEventInHeap => events.Min() != null;
    public F32 NextEventTime => events.Min().Key;
    public void RaiseFromHeap() => Raise(events.RemoveMin().Data);
    public void ClearHeap() => events.Clear();

    public void Raise<TEvent>(TEvent @event) where TEvent : IEvent {
      if (NeedExecuteImmediately)
        bus.Raise(@event);
      else
        events[time.CurrentTime] = @event;
    }

    public void RaiseSafely<TEvent>(TEvent @event) where TEvent : IEvent => bus.RaiseSafely(@event);

    readonly IEventBus bus;
    readonly FibonacciHeap.FibonacciHeap<IEvent, F32> events = 
      new FibonacciHeap.FibonacciHeap<IEvent, F32>(MinValue);
    ITime time;
  }
}