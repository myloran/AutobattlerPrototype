using Shared.Shared.Client;
using Shared.Shared.Client.Abstraction;
using UnityEngine;
using View.Exts;
using View.Presenters;

namespace View {
  public class MoveRoutine : ISimulationTick {
    public MoveRoutine(Transform unit, Vector3 from, Vector3 to, 
        float startTime, float duration) {
      this.unit = unit;
      this.from = from;
      this.to = to;
      this.duration = duration;
      this.startTime = startTime;
      endTime = startTime + duration;
    }
    
    public void SimulationTick(float time) {
      var timeClamped = Mathf.Clamp(time, startTime, endTime);
      var durationPassed = timeClamped - startTime;
      var t = durationPassed / duration;
      unit.transform.position = Vector3.Lerp(from, to, t);
    }

    readonly Transform unit;
    readonly Vector3 from;
    readonly Vector3 to;
    readonly float duration, startTime, endTime;
  }
}