using UnityEngine;

namespace Controller.Update {
  public class TickInput : MonoBehaviour {
    void Update() {
      if (isInit)
        tick.Tick();
    }

    public void Init(ITick tick) {
      this.tick = tick;
      isInit = true;
    }
    
    ITick tick;
    bool isInit;
  }
}