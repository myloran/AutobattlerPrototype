using UniRx;

namespace Controller.Update {
  public class TickController : ITick {
    public System.IObservable<long> OnUpdate;
    
    public void InitObservable(params ITick[] ticks) {
      this.ticks = ticks;
      OnUpdate = Observable.EveryUpdate();
    }

    public void Tick() {
      foreach (var t in ticks) 
        t.Tick();
    }

    ITick[] ticks;
  }
}