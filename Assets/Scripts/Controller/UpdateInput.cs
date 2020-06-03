using UnityEngine;

namespace Controller {
  public class UpdateInput : MonoBehaviour {
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