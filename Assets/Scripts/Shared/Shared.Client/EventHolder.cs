using PlasticFloor.EventBus;

namespace Shared.Shared.Client {
  public class EventHolder : IEventBus {
    public bool NeedExecuteImmediately = true;
      
    public EventHolder(IEventBus bus) {
      this.bus = bus;
    }
    
    public void Init(ITime time) => this.time = time;

    public bool HasEventInHeap => events.Min() != null;
    public TimePoint NextEventTime => events.Min().Key;
    public void RaiseFromHeap() => Raise(events.RemoveMin().Data);

    public void Raise<TEvent>(TEvent @event) where TEvent : IEvent {
      if (NeedExecuteImmediately)
        bus.Raise(@event);
      else
        events[time.CurrentTime] = @event;
    }

    public void RaiseSafely<TEvent>(TEvent @event) where TEvent : IEvent => bus.RaiseSafely(@event);

    readonly IEventBus bus;
    readonly FibonacciHeap.FibonacciHeap<IEvent, TimePoint> events = 
      new FibonacciHeap.FibonacciHeap<IEvent, TimePoint>(float.MinValue);
    ITime time;
  }
}