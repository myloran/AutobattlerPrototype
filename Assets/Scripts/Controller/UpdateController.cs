using Controller.NObserver;

namespace Controller {
  public class UpdateController : BaseObservable<ITick>, ITick {
    public UpdateController(params ITick[] ticks) {
      this.ticks = ticks;
    }

    public void Tick() {
      foreach (var observer in Observers) 
        observer.Observe<ITick>();

      foreach (var t in ticks) 
        t.Tick();
    }

    readonly ITick[] ticks;
  }
}