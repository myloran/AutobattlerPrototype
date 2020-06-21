using Controller.NObserver;
using UniRx;

namespace Controller.Update {
  public class TickController : BaseObservable<ITick>, ITick { //TODO: remove unused observer
    public System.IObservable<long> OnUpdate;
    
    public void Init(params ITick[] ticks) {
      this.ticks = ticks;
      OnUpdate = Observable.EveryUpdate();
    }

    public void Tick() {
      foreach (var observer in Observers) 
        observer.Observe<ITick>();

      foreach (var t in ticks) 
        t.Tick();
    }

    ITick[] ticks;
  }
}