using Shared.Abstraction;

namespace Controller.UnitDrag {
  public class CompositeHandler<T> : IHandler<T> {
    public CompositeHandler(params IHandler<T>[] handlers) {
      this.handlers = handlers;
    }
    
    public void Handle(T e) {
      foreach (var handler in handlers) {
        handler.Handle(e);
      }
    }

    readonly IHandler<T>[] handlers;
  }
}